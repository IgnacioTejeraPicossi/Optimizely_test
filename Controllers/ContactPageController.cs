using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models;

namespace OptiDemoCms.Controllers
{
    public class ContactPageController : PageController<ContactPage>
    {
        public IActionResult Index(ContactPage currentPage)
        {
            return View(currentPage);
        }
    }
}
