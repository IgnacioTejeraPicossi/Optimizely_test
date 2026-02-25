using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models.Blocks
{
    [ContentType(
        DisplayName = "Hero Block",
        GUID = "3f8c1e52-7f9a-4d6b-9a2b-3e4f1a2b6c9d",
        Description = "Simple hero block with heading, text and image")]
    public class HeroBlock : BlockData
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
        public virtual XhtmlString? Text { get; set; }

        [Display(
            Name = "Image",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [UIHint("Image")]
        public virtual ContentReference? Image { get; set; }
    }
}