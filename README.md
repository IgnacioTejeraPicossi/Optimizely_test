# Optimizely Demo CMS

Demo site on **Optimizely CMS 12 / .NET 8** for learning and testing (unit, integration, E2E).

## Implemented (roadmap phases 0–3)

- **Pages:** `StartPage` (MainHeading, HeroArea, TopContentArea, MainContentArea), `StandardPage` (Heading, MainBody, MainContentArea).
- **Blocks:** `HeroBlock` (Heading, Text, Image with UIHint), `RichTextBlock` (Heading, Body), `CTABlock` (Heading, Text, Link, ButtonLabel).
- **Layout:** `_Layout.cshtml` with header/nav/footer, `_ViewStart.cshtml`, `wwwroot/css/site.css`.
- **Navigation:** Menu from children of StartPage; `data-testid` on key elements for UI/integration tests.
- **Phase 3 (editorial UX):** Custom tab "Header" on StartPage; `[Required]` on MainHeading (StartPage) and Heading (StandardPage); `[AllowedTypes]` on ContentAreas — HeroArea allows only `HeroBlock`, TopContentArea/MainContentArea allow Hero, RichText, CTA blocks.
- **Integration tests:** `OptiDemoCms.Tests` with `WebApplicationFactory` — tests for `/` (200, HTML, start-page/main-heading/site-header/footer/nav and "Optimizely CMS is running"). Run with `dotnet test OptiDemoCms.Tests/OptiDemoCms.Tests.csproj` (stop any running `dotnet run` first).

## How to run

Chose one of the following options to get started. 

### Windows

Prerequisities
- .NET SDK 8+
- SQL Server 2016 Express LocalDB (or later)

```bash
$ dotnet run
````

### Any OS with Docker

Prerequisities
- Docker
- Enable Docker support when applying the template
- Review the .env file and make changes where necessary to the Docker-related variables

```bash
$ docker-compose up
````

> Note that this Docker setup is just configured for local development. Follow this [guide to enable HTTPS](https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md).

#### Reclaiming Docker Image Space

1. Backup the App_Data/\${DB_NAME}.mdf and App_Data/\${DB_NAME}.ldf DB restoration files for safety
2. Run `docker compose down --rmi all` to remove containers, networks, and images associated with the specific project instance
3. In the future, run `docker compose up` anytime you want to recreate the images and containers

### Any OS with external database server

Prerequisities
- .NET SDK 8+
- SQL Server 2016 (or later) on a external server, e.g. Azure SQL

Create an empty database on the external database server and update the connection string accordingly.

```bash
$ dotnet run
````
