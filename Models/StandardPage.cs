using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "Standard Page",
        GUID = "22222222-2222-2222-2222-222222222222",
        Description = "Standard page with heading, body and content area")]
    public class StandardPage : PageData
    {
        [Display(
            Name = "Heading",
            Description = "Page heading",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Heading { get; set; }

        [Display(
            Name = "Main body",
            Description = "Main body content",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString? MainBody { get; set; }

        [Display(
            Name = "Main content area",
            Description = "Content area for blocks",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual ContentArea? MainContentArea { get; set; }
    }
}
