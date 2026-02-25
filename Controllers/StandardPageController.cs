using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class StandardPageController : PageController<StandardPage>
    {
        public IActionResult Index(StandardPage currentPage)
        {
            return View(currentPage);
        }
    }
}
