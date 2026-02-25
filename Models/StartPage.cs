using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace OptiDemoCms.Models
{
    [ContentType(
        DisplayName = "Start Page",
        GUID = "11111111-1111-1111-1111-111111111111",
        Description = "Site start page")]
    public class StartPage : PageData
    {
        [Display(
            Name = "Main heading",
            Description = "Main heading of the page",
            GroupName = SystemTabNames.Content,
            Order = 10)]
         public virtual string? MainHeading { get; set; }

        [Display(
            Name = "Hero Area",
            Description = "Area for hero blocks",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual ContentArea? HeroArea { get; set; }

        [Display(
            Name = "Top content area",
            Description = "Content area above main content",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual ContentArea? TopContentArea { get; set; }

        [Display(
            Name = "Main content area",
            Description = "Main content area for blocks",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual ContentArea? MainContentArea { get; set; }
    }
}