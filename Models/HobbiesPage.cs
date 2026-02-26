using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "Hobbies Page",
        GUID = "a3333333-3333-3333-3333-333333333333",
        Description = "Personal hobbies page")]
    public class HobbiesPage : PageData
    {
        [Display(Name = "Heading", GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string? Heading { get; set; }

        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString? MainBody { get; set; }
    }
}
