using EPiServer;
using EPiServer.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;
using OptiDemoCms.Seed;

namespace OptiDemoCms.Controllers;

/// <summary>
/// Development-only endpoint to create personal pages under the Start Page.
/// Open https://localhost:5000/seed/personal-pages in the browser after the app is running.
/// </summary>
public class SeedController : Controller
{
    private readonly IContentRepository _contentRepository;
    private readonly IContentLoader _contentLoader;
    private readonly IWebHostEnvironment _env;

    public SeedController(
        IContentRepository contentRepository,
        IContentLoader contentLoader,
        IWebHostEnvironment env)
    {
        _contentRepository = contentRepository;
        _contentLoader = contentLoader;
        _env = env;
    }

    [HttpGet]
    [Route("seed/personal-pages")]
    public IActionResult PersonalPages()
    {
        if (!_env.IsDevelopment())
            return NotFound();

        var messages = new List<string>();

        var startRef = ContentReference.StartPage;
        if (startRef == null || startRef == ContentReference.EmptyReference)
        {
            return Content(
                "<html><body><h1>Seed: Personal pages</h1><p><strong>Start Page is not set.</strong> In CMS Admin, set your &quot;Hello Optimizely&quot; page as the site start page, then run <a href=\"/seed/personal-pages\">/seed/personal-pages</a> again.</p><p><a href=\"/\">Back to home</a></p></body></html>",
                "text/html");
        }

        if (!_contentLoader.TryGet<IContent>(startRef, out _))
        {
            return Content(
                "<html><body><h1>Seed: Personal pages</h1><p><strong>Start Page reference could not be loaded.</strong> Ensure the start page exists and is published. Then run <a href=\"/seed/personal-pages\">/seed/personal-pages</a> again.</p><p><a href=\"/\">Back to home</a></p></body></html>",
                "text/html");
        }

        var children = _contentLoader.GetChildren<PageData>(startRef).ToList();

        bool HasByName(string name) => children.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

        try
        {
            if (!HasByName("About Me"))
            {
                CreatePage<AboutMePage>(startRef, "About Me");
                messages.Add("Created: About Me");
            }
            else messages.Add("Already exists: About Me");

            if (!HasByName("AI tools"))
            {
                CreatePage<AIToolsPage>(startRef, "AI tools");
                messages.Add("Created: AI tools");
            }
            else messages.Add("Already exists: AI tools");

            if (!HasByName("My Hobbies"))
            {
                CreatePage<HobbiesPage>(startRef, "My Hobbies");
                messages.Add("Created: My Hobbies");
            }
            else messages.Add("Already exists: My Hobbies");

            if (!HasByName("Contact Me"))
            {
                CreatePage<ContactPage>(startRef, "Contact Me");
                messages.Add("Created: Contact Me");
            }
            else messages.Add("Already exists: Contact Me");
        }
        catch (Exception ex)
        {
            messages.Add("Error: " + ex.Message);
        }

        var list = string.Join("", messages.Select(m => "<li>" + System.Net.WebUtility.HtmlEncode(m) + "</li>"));
        return Content(
            "<html><body><h1>Seed: Personal pages</h1><ul>" + list + "</ul><p><strong>Refresh the home page</strong> to see the new menu items: <a href=\"/\">Home</a></p></body></html>",
            "text/html");
    }

    private void CreatePage<T>(ContentReference parentRef, string pageName) where T : PageData
    {
        var page = _contentRepository.GetDefault<T>(parentRef);
        page.Name = pageName;
        page.VisibleInMenu = true;
        _contentRepository.Save(page, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
    }
}
