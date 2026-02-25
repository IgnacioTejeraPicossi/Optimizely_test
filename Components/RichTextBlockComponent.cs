using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiDemoCms.Models.Blocks;

namespace OptiDemoCms.Components
{
    public class RichTextBlockComponent : BlockComponent<RichTextBlock>
    {
        protected override IViewComponentResult InvokeComponent(RichTextBlock currentContent)
        {
            return View("~/Views/Shared/Blocks/RichTextBlock.cshtml", currentContent);
        }
    }
}
