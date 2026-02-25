using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models.Blocks
{
    [ContentType(
        DisplayName = "Rich Text Block",
        GUID = "4a9c2e63-8f0b-4d7c-a3b4-4e5f2b3c7d0e",
        Description = "Block with heading and rich text content")]
    public class RichTextBlock : BlockData
    {
        [Display(
            Name = "Heading",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Heading { get; set; }

        [Display(
            Name = "Body",
            Description = "Rich text content",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString? Body { get; set; }
    }
}
