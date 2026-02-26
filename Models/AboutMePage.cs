using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "About Me Page",
        GUID = "a1111111-1111-1111-1111-111111111111",
        Description = "Personal About Me page with profile and experience")]
    public class AboutMePage : PageData
    {
        [Display(Name = "Heading", GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string? Heading { get; set; }

        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString? MainBody { get; set; }
    }
}
