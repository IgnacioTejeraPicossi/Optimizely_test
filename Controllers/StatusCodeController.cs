using EPiServer.Core;
using EPiServer.Filters;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    /// <summary>
    /// Handles status code pages (404, 500, etc.). Used by UseStatusCodePagesWithReExecute.
    /// Optionally loads a CMS page under StartPage named after the status code (e.g. "404").
    /// </summary>
    public class StatusCodeController : Controller
    {
        private readonly IContentLoader _contentLoader;

        public StatusCodeController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        [Route("/statuscode/{statusCode:int}")]
        [HttpGet]
        [HttpPost]
        public IActionResult Index(int statusCode)
        {
            Response.StatusCode = statusCode;

            if (statusCode == 404)
            {
                var startPageRef = ContentReference.StartPage;
                if (startPageRef != null)
                {
                    try
                    {
                        var notFoundPage = _contentLoader.GetChildren<NotFoundPage>(startPageRef)
                            .FirstOrDefault(p => string.Equals(p.Name, "404", StringComparison.OrdinalIgnoreCase));
                        if (notFoundPage != null && !new FilterContentForVisitor().ShouldFilter(notFoundPage))
                        {
                            return View("~/Views/NotFoundPage/Index.cshtml", notFoundPage);
                        }
                    }
                    catch
                    {
                        // Fall through to static view
                    }
                }
                return View("404");
            }

            return View(statusCode.ToString());
        }
    }
}
