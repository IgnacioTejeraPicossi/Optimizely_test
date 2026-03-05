# TESTING.md — OptiDemoCms

This document is the **single source of truth** for how to run and troubleshoot tests in OptiDemoCms.

> Recommended baseline: run `/health/ready` and wait for HTTP 200 before executing UI smoke tests.

---

## 0) Prerequisites

### Required
- .NET SDK 8+
- Node.js 18+
- A local database setup compatible with this app (LocalDB by default, or configured SQL Server)

### Recommended
- Chrome/Chromium installed (Playwright can install its own browsers)
- Git installed
- (Optional) Postman Desktop for API demos

---

## 1) Health checks (smoke hook)

These endpoints are used by automated tests and are safe to call anytime:

- `GET https://localhost:5000/health`
- `GET https://localhost:5000/health/ready`

Expected:
- HTTP 200 when healthy (HTTP 503 if unhealthy)
- `Content-Type: application/json`
- `/health` JSON includes `status`

### Quick manual check
Open `/health/ready` in a browser. If it is not 200, fix readiness before running E2E.

---

## 2) Integration tests (xUnit)

### Location
`OptiDemoCms.Tests/`

### Run
From repo root:

```bash
dotnet test OptiDemoCms.Tests/OptiDemoCms.Tests.csproj