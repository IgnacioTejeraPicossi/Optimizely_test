using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "AI Tools Page",
        GUID = "a2222222-2222-2222-2222-222222222222",
        Description = "Page listing AI tools")]
    public class AIToolsPage : PageData
    {
        [Display(Name = "Heading", GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string? Heading { get; set; }

        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString? MainBody { get; set; }
    }
}
