using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models.Blocks
{
    [ContentType(
        DisplayName = "CTA Block",
        GUID = "5b0d3f74-9g1c-5e8d-b4c5-5f6g3c4d8e1f",
        Description = "Call to action block with heading, text and button")]
    public class CTABlock : BlockData
    {
        [Display(
            Name = "Heading",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Heading { get; set; }

        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string? Text { get; set; }

        [Display(
            Name = "Link",
            Description = "Link for the button",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual LinkItemCollection? Link { get; set; }

        [Display(
            Name = "Button label",
            Description = "Text shown on the button",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual string? ButtonLabel { get; set; }
    }
}
