using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class AIToolsPageController : PageController<AIToolsPage>
    {
        public IActionResult Index(AIToolsPage currentPage)
        {
            return View(currentPage);
        }
    }
}
