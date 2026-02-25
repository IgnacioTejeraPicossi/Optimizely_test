using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class NotFoundPageController : PageController<NotFoundPage>
    {
        public IActionResult Index(NotFoundPage currentPage)
        {
            return View(currentPage);
        }
    }
}
