using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using OptiDemoCms;

namespace OptiDemoCms.Tests;

/// <summary>
/// Factory for integration tests. Uses the real Startup and content root from the main project.
/// Requires LocalDB / configured database to be available when running tests.
/// </summary>
public class OptiDemoCmsWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }
}
