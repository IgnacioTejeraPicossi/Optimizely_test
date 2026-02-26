using System.Net.Http.Headers;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using OptiDemoCms.Api;

namespace OptiDemoCms;

public class Startup
{
    private readonly IWebHostEnvironment _webHostingEnvironment;

    public Startup(IWebHostEnvironment webHostingEnvironment)
    {
        _webHostingEnvironment = webHostingEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCms()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>();

        services.AddHealthChecks()
            .AddCheck<Health.CmsReadinessHealthCheck>("cms_readiness", tags: new[] { "ready" });

        if (_webHostingEnvironment.IsDevelopment())
        {
            services.AddHostedService<Seed.PersonalPagesSeed>();
        }

        services.AddHttpClient("Groq", (sp, client) =>
        {
            var key = sp.GetRequiredService<IConfiguration>()["Ai:GroqApiKey"];
            if (!string.IsNullOrWhiteSpace(key))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        });
        services.AddHttpClient("HuggingFace", (sp, client) =>
        {
            var token = sp.GetRequiredService<IConfiguration>()["Ai:HuggingFaceToken"];
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        });
        services.AddSingleton<AiDemoService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseStatusCodePagesWithReExecute("/statuscode/{0}");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = OptiDemoCms.Health.HealthResponseWriter.WriteAsync
            });
            endpoints.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
                ResponseWriter = OptiDemoCms.Health.HealthResponseWriter.WriteAsync
            });
	    endpoints.MapControllers();
            endpoints.MapContent();
        });
    }
}
