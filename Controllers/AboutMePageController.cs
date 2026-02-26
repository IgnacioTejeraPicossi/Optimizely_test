using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class AboutMePageController : PageController<AboutMePage>
    {
        public IActionResult Index(AboutMePage currentPage)
        {
            return View(currentPage);
        }
    }
}
