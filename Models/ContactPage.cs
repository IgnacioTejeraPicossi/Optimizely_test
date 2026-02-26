using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "Contact Page",
        GUID = "a4444444-4444-4444-4444-444444444444",
        Description = "Contact Me page")]
    public class ContactPage : PageData
    {
        [Display(Name = "Heading", GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string? Heading { get; set; }

        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString? MainBody { get; set; }
    }
}
