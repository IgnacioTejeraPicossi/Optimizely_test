# Reproducibility and seed data

## Health as a test hook

- **`/health`**: overall status (all checks). Returns 200 if everything is Healthy, 503 if any Unhealthy. JSON with `status`, `totalDuration`, `entries`.
- **`/health/ready`**: only checks with tag `ready` (e.g. CMS readiness). Useful to know when the app is ready to receive traffic or to run tests that depend on the CMS.
  - **Healthy**: Start page configured and loadable.
  - **Degraded**: Start page configured but could not be loaded (e.g. not yet created in CMS).
  - **Unhealthy**: Start page not configured or check error.

In pipelines or orchestrators you can wait for `GET /health/ready` to return 200 before running smoke tests or sending traffic.

## Seed data (initial data)

Optimizely CMS does not include automatic content seeding in this project. To get a reproducible environment:

1. **First run:** `dotnet run`, open the site and CMS Admin, create the **Start page** (type Start Page) and set it as the site start page if needed.
2. **Reproducibility:** For identical environments (e.g. another developer or CI), options:
   - **Export/import:** Export content from the CMS (if available) and import it in the other environment.
   - **Script or IHostedService:** Create minimal content (Start page, 404 page) with `IContentRepository` in an initialization process (development); requires knowing the site structure and type GUIDs.
   - **Database:** Copy `App_Data/*.mdf` (and `.ldf`) to another machine after initial setup (development only; be careful not to mix environments).

For smoke tests, check `GET /health/ready` and, if it returns 200, assume the CMS is ready (Start page exists and is loadable).
