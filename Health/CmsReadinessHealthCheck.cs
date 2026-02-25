using EPiServer;
using EPiServer.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OptiDemoCms.Health;

/// <summary>
/// Readiness check: verifies that the CMS can resolve the start page (app is ready to serve content).
/// Useful for smoke tests and orchestration (e.g. Kubernetes readiness probe).
/// </summary>
public class CmsReadinessHealthCheck : IHealthCheck
{
    private readonly IContentLoader _contentLoader;

    public CmsReadinessHealthCheck(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var startPageRef = ContentReference.StartPage;
            if (startPageRef == null || startPageRef == ContentReference.EmptyReference)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    "Start page is not configured.",
                    data: new Dictionary<string, object> { ["startPageConfigured"] = false }));
            }

            if (!_contentLoader.TryGet<IContent>(startPageRef, out _))
            {
                return Task.FromResult(HealthCheckResult.Degraded(
                    "Start page reference is set but content could not be loaded (e.g. not yet created).",
                    data: new Dictionary<string, object> { ["startPageLoaded"] = false }));
            }

            return Task.FromResult(HealthCheckResult.Healthy(
                "CMS start page is available.",
                data: new Dictionary<string, object> { ["startPageLoaded"] = true }));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                "CMS readiness check failed.",
                ex,
                new Dictionary<string, object> { ["error"] = ex.Message }));
        }
    }
}
