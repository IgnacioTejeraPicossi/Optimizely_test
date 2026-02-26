using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OptiDemoCms.Models;

namespace OptiDemoCms.Seed;

/// <summary>
/// Creates the personal pages (About Me, AI tools, My Hobbies, Contact Me) under the Start Page
/// if they do not exist. Runs once at startup in Development. You can still publish or edit them in the CMS.
/// </summary>
public class PersonalPagesSeed : IHostedService
{
    private readonly IContentRepository _contentRepository;
    private readonly IContentLoader _contentLoader;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<PersonalPagesSeed> _logger;

    public PersonalPagesSeed(
        IContentRepository contentRepository,
        IContentLoader contentLoader,
        IWebHostEnvironment env,
        ILogger<PersonalPagesSeed> logger)
    {
        _contentRepository = contentRepository;
        _contentLoader = contentLoader;
        _env = env;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_env.IsDevelopment())
            return Task.CompletedTask;

        _ = Task.Run(async () =>
        {
            await Task.Delay(3000, cancellationToken);
            try
            {
                EnsurePersonalPagesExist();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Personal pages seed failed. Create About Me, AI tools, My Hobbies, Contact Me manually under Start Page in CMS.");
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void EnsurePersonalPagesExist()
    {
        var startRef = ContentReference.StartPage;
        if (startRef == null || startRef == ContentReference.EmptyReference)
            return;

        if (!_contentLoader.TryGet<IContent>(startRef, out _))
            return;

        var children = _contentLoader.GetChildren<PageData>(startRef).ToList();

        bool HasByName(string name) => children.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

        if (!HasByName("About Me"))
            CreatePage<AboutMePage>(startRef, "About Me");
        if (!HasByName("AI tools"))
            CreatePage<AIToolsPage>(startRef, "AI tools");
        if (!HasByName("My Hobbies"))
            CreatePage<HobbiesPage>(startRef, "My Hobbies");
        if (!HasByName("Contact Me"))
            CreatePage<ContactPage>(startRef, "Contact Me");
    }

    private void CreatePage<T>(ContentReference parentRef, string pageName) where T : PageData
    {
        var page = _contentRepository.GetDefault<T>(parentRef);
        page.Name = pageName;
        page.VisibleInMenu = true;
        _contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess);
        _logger.LogInformation("Created page: {Name} ({Type})", pageName, typeof(T).Name);
    }
}
