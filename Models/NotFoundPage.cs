using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "Not Found Page",
        GUID = "33333333-3333-3333-3333-333333333333",
        Description = "Page shown when content is not found (404)")]
    public class NotFoundPage : PageData
    {
        [Display(
            Name = "Heading",
            Description = "Heading shown on 404 page",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [CultureSpecific]
        public virtual string? Heading { get; set; }

        [Display(
            Name = "Message",
            Description = "Message shown to the user",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [CultureSpecific]
        public virtual XhtmlString? Message { get; set; }
    }
}
