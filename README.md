# Optimizely Demo CMS

Demo site on **Optimizely CMS 12 / .NET 8** for learning and practising **testing** (unit, integration, E2E, API tests with Postman), **accessibility** (WAVE), and **AI API** integration (Groq, Hugging Face). Based on a phased roadmap (0–5) implemented to date. Includes personal pages (About Me, AI tools, My Hobbies, Contact Me) with sidebar layout, icons, contact form, seed for content, Cypress and Playwright E2E, and a Postman collection for the AI API.

---

## Current status: phases 0–5 completed

### Phase 0 — Baseline
- Standard structure: `Models/Pages`, `Models/Blocks` (folder `Blokcs`), `Controllers`, `Views/Pages`, `Views/Shared/Blocks`, `wwwroot`.
- StartPage at `/` with MainHeading and HeroArea.
- `launchSettings.json` and `appsettings.Development.json` configured.
- Project builds and runs without critical warnings.

### Phase 1 — Content model (Pages, Blocks)
- **Pages**
  - **StartPage:** MainHeading, HeroArea, TopContentArea, MainContentArea.
  - **StandardPage:** Heading, MainBody (XhtmlString), MainContentArea.
  - **NotFoundPage:** Heading, Message (XhtmlString); used for 404.
- **Blocks**
  - **HeroBlock:** Heading, Text (XhtmlString), Image (ContentReference with UIHint "Image").
  - **RichTextBlock:** Heading, Body (XhtmlString).
  - **CTABlock:** Heading, Text, Link (LinkItemCollection), ButtonLabel.
- **Controllers and views:** StartPageController, StandardPageController, NotFoundPageController; views in `Views/StartPage`, `Views/StandardPage`, `Views/NotFoundPage`. Blocks rendered via components (`HeroBlockComponent`, `RichTextBlockComponent`, `CTABlockComponent`) and views in `Views/Shared/Blocks`.
- **Personal pages (menu: About Me, AI tools, My Hobbies, Contact Me):** **AboutMePage**, **AIToolsPage**, **HobbiesPage**, **ContactPage** with sidebar layout (`_PersonalLayout.cshtml`, `wwwroot/css/personal.css`). Pre-filled content: profile (certifications, work experience, skills), AI tools list with icons (`wwwroot/images/ai-tools/`), hobbies list with icons (`wwwroot/images/hobbies/`). **Contact Me** includes contact details (phone, email), a contact form (name, email, message, “Send Message”), POST to `/contact/send` (thank-you message), and footer “Designed by Ignacio Tejera, Item Consulting.” **Seed:** In Development, visit `/seed/personal-pages` once to create the four personal pages under the Start Page if they don’t exist; or use **SeedController** + **PersonalPagesSeed** (IHostedService) which runs after a short delay at startup.

### Phase 2 — Layout and navigation
- **Layout:** `Views/Shared/_Layout.cshtml` with header, nav, main, footer; `_ViewStart.cshtml` applies the default layout.
- **Navigation:** menu from children of StartPage (IContentLoader, FilterContentForVisitor, VisibleInMenu); Search link in the menu.
- **Styles:** `wwwroot/css/site.css` (CSS variables, header, nav, hero, CTA, footer, 404, search).
- **SEO in head:** title from ViewData; meta description and canonical when views set them.

### Phase 3 — Editorial rules and CMS UX
- **Tabs:** On StartPage, **"Header"** tab (MainHeading, HeroArea) and **"Content"** tab (TopContentArea, MainContentArea); **"SEO"** tab on StartPage and StandardPage.
- **Validation:** `[Required]` on MainHeading (StartPage) and Heading (StandardPage).
- **ContentArea restrictions:**
  - **HeroArea:** only `HeroBlock` (`[AllowedTypes(typeof(HeroBlock))]`).
  - **TopContentArea and MainContentArea (StartPage), MainContentArea (StandardPage):** HeroBlock, RichTextBlock, CTABlock.
- Display, Description and Order consistent across models.

### Phase 4 — SEO, 404, search, localization
- **SEO**
  - StartPage and StandardPage: **MetaTitle**, **MetaDescription**, **CanonicalUrl** properties (SEO tab).
  - Layout: `<title>`, `<meta name="description">`, `<link rel="canonical">` from ViewData (page views set MetaTitle, MetaDescription, CanonicalUrl).
- **404 and errors**
  - **NotFoundPage** content type (Heading, Message).
  - **StatusCodeController** at `/statuscode/{statusCode}`; for 404 it tries to load a CMS page of type NotFoundPage named "404" under StartPage; if none exists, uses the static view `Views/StatusCode/404.cshtml`. Response sets `Content-Type: text/html; charset=utf-8` for E2E (e.g. Cypress).
  - `UseStatusCodePagesWithReExecute("/statuscode/{0}")` in Startup (placed before UseRouting so re-execute works correctly).
- **Search**
  - Route **/search**, parameter **q**; simple search by title across all pages that are descendants of StartPage (recursive with IContentLoader).
  - SearchController + view with form and results list; IUrlResolver for URLs.
- **Localization**
  - `[CultureSpecific]` on: MainHeading, Heading, MetaTitle, MetaDescription (StartPage and StandardPage); Heading and Message (NotFoundPage).
  - Languages are enabled in CMS Admin (Manage websites → Languages).

### Phase 5 — Observability and test hooks
- **Health endpoint**
  - **`/health`**: overall status (all checks). JSON response with `status`, `totalDuration`, `entries`. 200 if Healthy, 503 if Unhealthy.
  - **`/health/ready`**: only checks with tag `ready` (readiness for orchestration). Includes **CmsReadinessHealthCheck**: verifies that the Start page is configured and loadable (Healthy/Degraded/Unhealthy).
  - Response in JSON (HealthResponseWriter) for smoke tests and orchestrators.
- **Structured logging**
  - **Serilog** configured in `Program.cs` and `appsettings.json` / `appsettings.Development.json`.
  - Enrichment: `FromLogContext`, `Application` property.
  - In Development: level `Debug` for `OptiDemoCms`; in production, `Information`/`Warning` per configuration.
- **Reproducibility and seed data**
  - Documentation in **`Docs/Reproducibility.md`**: use of `/health` and `/health/ready` as test hooks, seed data options (export/import, script, DB copy), and recommendation to wait for `/health/ready` to return 200 before smoke tests.
  - Start page is created in the CMS on first run. Personal pages (About Me, AI tools, My Hobbies, Contact Me) can be auto-created in Development via **PersonalPagesSeed** or manually via **`/seed/personal-pages`**.

### Testing (integration tests)
- **Project:** `OptiDemoCms.Tests` (xUnit, FluentAssertions, Microsoft.AspNetCore.Mvc.Testing).
- **Factory:** `OptiDemoCmsWebApplicationFactory` inherits from `WebApplicationFactory<Startup>`.
- **Tests in HomePageIntegrationTests:**
  - `/` returns 200 and HTML.
  - Response contains: `data-testid="start-page"`, `data-testid="main-heading"`, `data-testid="main-content"`, `data-testid="site-header"`, `data-testid="site-footer"`, `data-testid="site-nav"` and the text "Optimizely CMS is running".
- **Tests in HealthEndpointIntegrationTests (smoke):**
  - `/health` and `/health/ready` return 200 or 503 and `application/json`.
  - Response body of `/health` contains the `status` property.
- **Run tests:** `dotnet test OptiDemoCms.Tests/OptiDemoCms.Tests.csproj`  
  **Important:** stop any running `dotnet run` before running tests (the executable is locked).
- The test project folder is excluded from the main project (`OptiDemoCms.csproj`) so its `.cs` files are not compiled into the web app.

### Cypress E2E (UI tests)
- **Project:** `cypress-e2e/` (Node.js; Cypress 13).
- **Prerequisites:** Node.js 18+, app running at `https://localhost:5000`.
- **Setup:** `cd cypress-e2e && npm install`
- **Run:** From the `cypress-e2e` folder run `npm run cy:open` (interactive) or `npm run cy:run` (headless).
- **Specs:** `home.cy.js` (StartPage), `search.cy.js`, `not-found.cy.js`, `health.cy.js`, `personal-pages.cy.js`; use `data-testid` selectors.
- See **`cypress-e2e/README.md`** for details.

### Playwright E2E (public + CMS UI)
- **Project:** `tests-playwright/` (Node.js; Playwright, TypeScript).
- **Prerequisites:** Node.js 18+, app running at `https://localhost:5000`.
- **Setup:** `cd tests-playwright && npm install && npx playwright install chromium`
- **Run:** `npm run test` (all), `npm run test:public` (public only), `npm run test:ui` (UI mode). CMS tests require running `npm run cms:login` first (set `CMS_USER` / `CMS_PASS`).
- **Specs:** Public — `home.spec.ts`, `navigation.spec.ts`, `hero-block-render.spec.ts`, `home.visual.spec.ts`; CMS — `login.setup.ts`, `create-hero-block.spec.ts`, `add-block-to-home.spec.ts`, `publish-home.spec.ts` (scaffold; use codegen to complete).
- Anti-flakiness: `test-utils/stabilize.ts` (animations off, networkidle), `ignoreHTTPSErrors`, deterministic waits.
- See **`tests-playwright/README.md`** for details.

### AI Demo API (Postman)
- **Endpoints:** `GET /api/ai/health`, `POST /api/ai/complete` (Groq/Llama), `POST /api/ai/summarize` and `POST /api/ai/sentiment` (Hugging Face).
- **Demo mode:** All work without API keys (sample responses for Postman demos). Set **Ai:GroqApiKey** and/or **Ai:HuggingFaceToken** (or env `Ai__GroqApiKey`, `Ai__HuggingFaceToken`) for live AI.
- **Postman:** Import **`postman/Optimizely-Demo-AI-API.postman_collection.json`** and set variable `baseUrl` to your app URL (e.g. `https://localhost:5000`). See **`Docs/Api-AI-Postman.md`** for details and API key setup (Groq, Hugging Face).

**API tests in Postman – AI health (200 OK, demo mode)**

![Postman – Optimizely Demo AI API collection, Get AI health 200 OK, status, mode, providers, endpoints](Docs/screenshots/postman-ai-api-health.png)

### Ready for more tests
- **data-testid** in views: start-page, main-heading, hero-area, main-content-area, site-header, site-nav, site-footer, standard-page, page-heading, main-body, rich-text-block, cta-block, cta-button, search-page, search-form, search-input, search-submit, search-results, not-found-page, not-found-heading, not-found-home-link, etc.

---

## Project structure (summary)

```
Optimizely/
├── Api/                   AiDemoService (Groq, Hugging Face)
├── Controllers/           StartPage, StandardPage, NotFoundPage, AboutMe, AITools, Hobbies, Contact, ContactPage, Search, StatusCode, Seed, AiApi
├── Components/            HeroBlock, RichTextBlock, CTABlock
├── Health/                CmsReadinessHealthCheck, HealthResponseWriter
├── Models/                StartPage, StandardPage, NotFoundPage, AboutMePage, AIToolsPage, HobbiesPage, ContactPage
├── Models/Blokcs/         HeroBlock, RichTextBlock, CTABlock
├── Seed/                  PersonalPagesSeed (IHostedService); creates About Me, AI tools, My Hobbies, Contact Me under Start Page
├── Views/
│   ├── StartPage/         Index.cshtml
│   ├── StandardPage/      Index.cshtml
│   ├── NotFoundPage/      Index.cshtml
│   ├── AboutMePage/       Index.cshtml
│   ├── AIToolsPage/       Index.cshtml (list + icons from wwwroot/images/ai-tools/)
│   ├── HobbiesPage/       Index.cshtml (list + icons from wwwroot/images/hobbies/)
│   ├── ContactPage/       Index.cshtml (contact details, form, footer)
│   ├── Search/            Index.cshtml
│   ├── Shared/            _Layout.cshtml, _PersonalLayout.cshtml, Blocks/*.cshtml
│   └── StatusCode/        404.cshtml
├── Docs/                  Reproducibility.md, Api-AI-Postman.md
├── Docs/screenshots/      wave-accessibility-home.png, postman-ai-api-health.png
├── postman/               Optimizely-Demo-AI-API.postman_collection.json (GET /api/ai/health, POST complete, summarize, sentiment)
├── wwwroot/
│   ├── css/               site.css, personal.css
│   └── images/            ai-tools/ (icons per tool), hobbies/ (icons per hobby)
├── OptiDemoCms.Tests/      WebApplicationFactory, HomePageIntegrationTests, HealthEndpointIntegrationTests
├── cypress-e2e/           Cypress E2E (home, search, not-found, health, personal-pages); run from cypress-e2e folder
├── tests-playwright/      Playwright E2E (public + CMS UI; stabilize, visual snapshots); run from tests-playwright folder
├── Program.cs, Startup.cs, OptiDemoCms.csproj
└── README.md
```

---

## How to run

### Windows
- **Requirements:** .NET SDK 8+, SQL Server 2016 Express LocalDB (or later).
- `dotnet run`

### Docker
- **Requirements:** Docker; review variables in `.env` if applicable.
- `docker-compose up`  
- For local development only; see [HTTPS guide](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md) if needed.

### External database
- **Requirements:** .NET SDK 8+, SQL Server on an external server (e.g. Azure SQL).
- Create an empty database and update the connection string; then `dotnet run`.

---

## Suggested next steps

- **Tests:** Refine integration tests (StandardPage, /search, 404); unit tests (model validation); extend Playwright CMS tests with codegen.
- **Contact form:** Optional: wire `/contact/send` to real email (e.g. IEmailSender).

---

## Accessibility (WAVE)

First accessibility check with [WAVE](https://wave.webaim.org/) (WebAIM) on the home page: no errors, no contrast errors, no alerts. Manual testing is still recommended for full compliance.

![WAVE accessibility evaluation on Optimizely Demo CMS home page — no errors detected](Docs/screenshots/wave-accessibility-home.png)

---

## Summary for agents and LLMs (AGENTS.md / llms.txt / testing.md)

This section gives a compact overview for generating or maintaining project docs (e.g. AGENTS.md, llms.txt, testing.md).

**Tech stack:** Optimizely CMS 12, ASP.NET Core / .NET 8, C#, Razor views, SQL Server (LocalDB by default), Serilog, EPiServer namespaces for content (IContentLoader, IContentRepository, ContentReference, PageData, etc.).

**Entry points:** `Program.cs` (host, Serilog), `Startup.cs` (ConfigureServices, Configure with middleware and MapControllers + MapContent). Main app URL: `https://localhost:5000` (see `Properties/launchSettings.json`).

**Public routes:** `/` (StartPage), `/search`, `/contact-me/`, `/about-me/`, `/ai-tools/`, `/my-hobbies/`, `/health`, `/health/ready`, `/seed/personal-pages` (Development), `/api/ai/health`, `/api/ai/complete`, `/api/ai/summarize`, `/api/ai/sentiment`, `/contact/send` (POST). 404 handled via `UseStatusCodePagesWithReExecute` and `StatusCodeController` with `Content-Type: text/html`.

**Testing strategy:**
- **Integration (xUnit):** `OptiDemoCms.Tests` with `WebApplicationFactory<Startup>`. Tests hit `/`, `/health`, `/health/ready` and assert status, content-type, and `data-testid` / JSON. Stop `dotnet run` before `dotnet test`.
- **Cypress E2E:** `cypress-e2e/` (Node, Cypress 13). Specs: home, search, not-found, health, personal-pages. Run from **inside** `cypress-e2e` (`npm run cy:open` or `npm run cy:run`). Base URL `https://localhost:5000`; `data-testid` used for selectors.
- **Playwright E2E:** `tests-playwright/` (Node, TypeScript). Public specs: home, navigation, hero-block-render, home.visual (snapshots). CMS specs (login.setup, create-hero-block, etc.) are scaffold; use codegen to complete. Run from **inside** `tests-playwright`; base URL `https://localhost:5000`.
- **API (Postman):** Collection in `postman/Optimizely-Demo-AI-API.postman_collection.json`. Tests can be added in the request Scripts (Tests) tab; results appear under the response “Test Results” tab.

**Conventions:** Prefer `data-testid` in views for stable E2E selectors. API keys for AI (Groq, Hugging Face) via `Ai:GroqApiKey`, `Ai:HuggingFaceToken` or env `Ai__*`. Personal pages created under Start Page (seed or manually in CMS).
