using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class HobbiesPageController : PageController<HobbiesPage>
    {
        public IActionResult Index(HobbiesPage currentPage)
        {
            return View(currentPage);
        }
    }
}
