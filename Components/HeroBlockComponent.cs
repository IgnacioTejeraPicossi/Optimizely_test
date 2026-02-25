using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models.Blocks;

namespace OptiDemoCms.Components
{
    public class HeroBlockComponent : BlockComponent<HeroBlock>
    {
        protected override IViewComponentResult InvokeComponent(HeroBlock currentContent)
        {
            return View("~/Views/Shared/Blocks/HeroBlock.cshtml", currentContent);
        }
    }
}