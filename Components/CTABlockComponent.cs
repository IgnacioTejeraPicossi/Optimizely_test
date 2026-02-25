using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models.Blocks;

namespace OptiDemoCms.Components
{
    public class CTABlockComponent : BlockComponent<CTABlock>
    {
        protected override IViewComponentResult InvokeComponent(CTABlock currentContent)
        {
            return View("~/Views/Shared/Blocks/CTABlock.cshtml", currentContent);
        }
    }
}
