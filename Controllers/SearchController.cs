using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class SearchController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IUrlResolver _urlResolver;

        public SearchController(IContentLoader contentLoader, IUrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Index(string? q)
        {
            var model = new SearchResultViewModel { Query = q ?? string.Empty };

            if (string.IsNullOrWhiteSpace(q))
            {
                return View(model);
            }

            var startPageRef = ContentReference.StartPage;
            if (startPageRef == null)
            {
                return View(model);
            }

            var filter = new FilterContentForVisitor();
            var allPages = GetDescendants<PageData>(startPageRef)
                .Where(p => !filter.ShouldFilter(p))
                .ToList();

            var query = q.Trim();
            model.Results = allPages
                .Where(p => (p.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
                .Select(p => new SearchResultItem
                {
                    Title = p.Name,
                    Url = _urlResolver.GetUrl(p.ContentLink) ?? "#"
                })
                .ToList();

            return View(model);
        }

        private IEnumerable<T> GetDescendants<T>(ContentReference parentRef) where T : IContent
        {
            var children = _contentLoader.GetChildren<T>(parentRef);
            foreach (var child in children)
            {
                yield return child;
                foreach (var descendant in GetDescendants<T>(child.ContentLink))
                {
                    yield return descendant;
                }
            }
        }
    }

    public class SearchResultViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<SearchResultItem> Results { get; set; } = new();
    }

    public class SearchResultItem
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
    }
}
