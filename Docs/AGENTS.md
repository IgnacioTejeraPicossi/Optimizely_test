# AGENTS.md — OptiDemoCms

This file defines how AI agents (Cursor, CLI agents, MCP-enabled agents) must operate when making changes to this repository.

## Project summary
OptiDemoCms is a demo site built on **Optimizely CMS 12 / ASP.NET Core / .NET 8** for learning and practicing:
- Testing (unit/integration/E2E, Postman API tests)
- Accessibility checks (WAVE)
- AI API integration demos (Groq + Hugging Face)

Key public routes:
- `/` (StartPage)
- `/search`
- `/about-me/`, `/ai-tools/`, `/my-hobbies/`, `/contact-me/`
- `/health`, `/health/ready`
- `/seed/personal-pages` (Development only)
- `/api/ai/*` endpoints (demo mode supported)

## Agent operating principles (non-negotiable)
1. **Make small, reviewable changes.** Prefer multiple small commits/PRs over “big bang” rewrites.
2. **Do not break editor & CMS workflows.** Any change that impacts Optimizely content types, templates, or routing must be verified by at least one automated test (integration and/or E2E).
3. **Preserve deterministic selectors.** Prefer `data-testid` attributes for any UI element targeted by Cypress/Playwright.
4. **Never commit secrets.** Do not add API keys/tokens to files. Use appsettings placeholders + environment variables.
5. **Keep the app runnable on Windows with LocalDB.** Do not introduce dependencies that require Linux-only tooling unless explicitly requested.
6. **Respect existing architecture.** Controllers, models (Pages/Blocks), views, components, and health checks follow established conventions in this repo.

## “How to work” workflow (recommended)
When implementing a task:
1. **Clarify scope** in 1–3 bullets in your own notes (what files you expect to touch, which tests should validate it).
2. **Locate the right layer**:
   - Page types / block types: `Models/Pages`, `Models/Blokcs` (note folder spelling)
   - Rendering: `Views/**`, `Views/Shared/Blocks/**`
   - Block rendering components: `Components/**`
   - Routes and behavior: `Controllers/**`
   - Health: `Health/**`
   - Seed: `Seed/**`, `Controllers/SeedController.cs`
3. **Implement minimally** (prefer extending current patterns rather than introducing new frameworks).
4. **Run relevant tests** (see `TESTING.md`):
   - `dotnet test` for integration
   - Playwright/Cypress for UI behavior
5. **Summarize the change**:
   - What changed
   - Why
   - Which tests were run + results
   - Any risk/edge cases

## Guardrails for common change types

### Content models (Pages/Blocks)
- Keep Display/Description/Order consistent.
- If you add a property used in views, add/update:
  - one integration test assertion OR
  - one E2E test assertion (prefer `data-testid`).

### Razor views and layouts
- Prefer adding `data-testid` to support stable E2E selectors.
- Avoid fragile selectors (classes that may change with styling).

### Search / 404 behavior
- `/search` should remain stable and safe for empty queries.
- 404 should continue to serve HTML with correct `Content-Type`.
- If you change 404 behavior, update integration tests and at least one E2E check.

### Health endpoints
- `/health` and `/health/ready` must return JSON with:
  - HTTP 200 when healthy, 503 when unhealthy
  - `Content-Type: application/json`
- If you add health checks, ensure readiness remains meaningful for orchestrators and smoke tests.

### AI API endpoints (`/api/ai/*`)
- Preserve demo mode behavior (works without keys).
- Add keys via configuration:
  - `Ai:GroqApiKey`, `Ai:HuggingFaceToken` OR env `Ai__GroqApiKey`, `Ai__HuggingFaceToken`.
- If response schema changes, update Postman docs and tests accordingly.

## When to use CLI tools vs GUI editor
- GUI (Cursor) is preferred for:
  - multi-file refactors with human review
  - Razor + model edits where visual diff review matters
- CLI agents are preferred for:
  - repetitive test authoring (Playwright/Cypress) and updating selectors
  - mechanical refactors (rename/move) followed by full test run

## If MCP is available (optional)
If the environment supports MCP tooling, prioritize:
- Playwright runs with trace collection
- Screenshot capture for failures
- Console/network logging for debugging

But MCP tool calls must be treated as **non-authoritative** unless validated by repo tests.

## “Stop conditions” (when to halt and report)
Stop and report clearly if:
- Build fails after minimal fixes
- Tests fail with unclear root cause after reasonable investigation
- You detect conflicting conventions (e.g., content types vs views mismatch)
- You are about to change database/schema behavior without explicit request

## Definition of Done (DoD) for agent contributions
A change is “done” when:
- Build succeeds
- Relevant tests pass (minimum: integration tests; plus E2E when UI behavior changed)
- No secrets committed
- Update docs if a developer workflow changed
- A short summary is included with test evidence