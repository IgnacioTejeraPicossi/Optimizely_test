# Optimizely Demo CMS

Sitio demo en **Optimizely CMS 12 / .NET 8** para aprender y practicar testing (unit, integration, E2E). Basado en un roadmap por fases (0–4) implementado hasta la fecha.

---

## Estado actual: fases 0–4 completadas

### Fase 0 — Baseline
- Estructura estándar: `Models/Pages`, `Models/Blocks` (carpeta `Blokcs`), `Controllers`, `Views/Pages`, `Views/Shared/Blocks`, `wwwroot`.
- StartPage en `/` con MainHeading y HeroArea.
- `launchSettings.json` y `appsettings.Development.json` configurados.
- Proyecto compila y arranca sin warnings críticos.

### Fase 1 — Modelo de contenido (Pages, Blocks)
- **Pages**
  - **StartPage:** MainHeading, HeroArea, TopContentArea, MainContentArea.
  - **StandardPage:** Heading, MainBody (XhtmlString), MainContentArea.
  - **NotFoundPage:** Heading, Message (XhtmlString); usado para 404.
- **Blocks**
  - **HeroBlock:** Heading, Text (XhtmlString), Image (ContentReference con UIHint "Image").
  - **RichTextBlock:** Heading, Body (XhtmlString).
  - **CTABlock:** Heading, Text, Link (LinkItemCollection), ButtonLabel.
- **Controladores y vistas:** StartPageController, StandardPageController, NotFoundPageController; vistas en `Views/StartPage`, `Views/StandardPage`, `Views/NotFoundPage`. Bloques renderizados vía componentes (`HeroBlockComponent`, `RichTextBlockComponent`, `CTABlockComponent`) y vistas en `Views/Shared/Blocks`.

### Fase 2 — Layout y navegación
- **Layout:** `Views/Shared/_Layout.cshtml` con header, nav, main, footer; `_ViewStart.cshtml` aplica el layout por defecto.
- **Navegación:** menú con hijos de StartPage (IContentLoader, FilterContentForVisitor, VisibleInMenu); enlace a Search en el menú.
- **Estilos:** `wwwroot/css/site.css` (variables CSS, header, nav, hero, CTA, footer, 404, search).
- **SEO en head:** título desde ViewData; meta description y canonical cuando las vistas los rellenan.

### Fase 3 — Reglas editoriales y UX en CMS
- **Tabs:** En StartPage, pestaña **"Header"** (MainHeading, HeroArea) y **"Content"** (TopContentArea, MainContentArea); pestaña **"SEO"** en StartPage y StandardPage.
- **Validación:** `[Required]` en MainHeading (StartPage) y Heading (StandardPage).
- **Restricciones en ContentArea:**
  - **HeroArea:** solo `HeroBlock` (`[AllowedTypes(typeof(HeroBlock))]`).
  - **TopContentArea y MainContentArea (StartPage), MainContentArea (StandardPage):** HeroBlock, RichTextBlock, CTABlock.
- Display, Description y Order coherentes en los modelos.

### Fase 4 — SEO, 404, búsqueda, localización
- **SEO**
  - StartPage y StandardPage: propiedades **MetaTitle**, **MetaDescription**, **CanonicalUrl** (pestaña SEO).
  - Layout: `<title>`, `<meta name="description">`, `<link rel="canonical">` según ViewData (las vistas de página rellenan MetaTitle, MetaDescription, CanonicalUrl).
- **404 y errores**
  - Tipo de contenido **NotFoundPage** (Heading, Message).
  - **StatusCodeController** en `/statuscode/{statusCode}`; para 404 intenta cargar una página CMS de tipo NotFoundPage llamada "404" bajo StartPage; si no existe, usa la vista estática `Views/StatusCode/404.cshtml`.
  - `UseStatusCodePagesWithReExecute("/statuscode/{0}")` en Startup.
- **Búsqueda**
  - Ruta **/search**, parámetro **q**; búsqueda simple por título en todas las páginas descendientes de StartPage (recursivo con IContentLoader).
  - SearchController + vista con formulario y lista de resultados; IUrlResolver para URLs.
- **Localización**
  - `[CultureSpecific]` en: MainHeading, Heading, MetaTitle, MetaDescription (StartPage y StandardPage); Heading y Message (NotFoundPage).
  - Idiomas se activan en CMS Admin (Manage websites → Languages).

### Testing (integration tests)
- **Proyecto:** `OptiDemoCms.Tests` (xUnit, FluentAssertions, Microsoft.AspNetCore.Mvc.Testing).
- **Factory:** `OptiDemoCmsWebApplicationFactory` hereda de `WebApplicationFactory<Startup>`.
- **Tests en HomePageIntegrationTests:**
  - `/` devuelve 200 y HTML.
  - La respuesta contiene: `data-testid="start-page"`, `data-testid="main-heading"`, `data-testid="main-content"`, `data-testid="site-header"`, `data-testid="site-footer"`, `data-testid="site-nav"` y el texto "Optimizely CMS is running".
- **Ejecución:** `dotnet test OptiDemoCms.Tests/OptiDemoCms.Tests.csproj`  
  **Importante:** cerrar cualquier `dotnet run` antes de ejecutar los tests (el ejecutable queda bloqueado).
- La carpeta del proyecto de tests está excluida del proyecto principal (`OptiDemoCms.csproj`) para que sus `.cs` no se compilen en la web app.

### Elementos listos para más tests
- **data-testid** en vistas: start-page, main-heading, hero-area, main-content-area, site-header, site-nav, site-footer, standard-page, page-heading, main-body, rich-text-block, cta-block, cta-button, search-page, search-form, search-input, search-submit, search-results, not-found-page, not-found-heading, not-found-home-link, etc.

---

## Estructura del proyecto (resumen)

```
Optimizely/
├── Controllers/          StartPage, StandardPage, NotFoundPage, Search, StatusCode
├── Components/           HeroBlock, RichTextBlock, CTABlock
├── Models/               StartPage, StandardPage, NotFoundPage
├── Models/Blokcs/        HeroBlock, RichTextBlock, CTABlock
├── Views/
│   ├── StartPage/        Index.cshtml
│   ├── StandardPage/     Index.cshtml
│   ├── NotFoundPage/     Index.cshtml
│   ├── Search/           Index.cshtml
│   ├── Shared/           _Layout.cshtml, Blocks/*.cshtml
│   └── StatusCode/       404.cshtml
├── wwwroot/css/          site.css
├── OptiDemoCms.Tests/    WebApplicationFactory, HomePageIntegrationTests
├── Program.cs, Startup.cs, OptiDemoCms.csproj
└── README.md
```

---

## Cómo ejecutar

### Windows
- **Requisitos:** .NET SDK 8+, SQL Server 2016 Express LocalDB (o superior).
- `dotnet run`

### Docker
- **Requisitos:** Docker; revisar variables en `.env` si aplica.
- `docker-compose up`  
- Solo desarrollo local; ver [guía HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md) si hace falta.

### Base de datos externa
- **Requisitos:** .NET SDK 8+, SQL Server en servidor externo (p. ej. Azure SQL).
- Crear base de datos vacía y actualizar la connection string; luego `dotnet run`.

---

## Próximos pasos sugeridos (roadmap original)

- **Fase 5:** Observabilidad y “test hooks” (logging estructurado, health endpoint, seed data, más data-testid).
- **Tests:** Afinar integration tests (StandardPage, /search, 404); unit tests (validación de modelos); opcional: Playwright para smoke E2E.
