using EPiServer.Core;
using EPiServer.DataAnnotations;
using OptiDemoCms.Models.Blocks;
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
            GroupName = "Header",
            Order = 10)]
        [Required(ErrorMessage = "Main heading is required")]
        public virtual string? MainHeading { get; set; }

        [Display(
            Name = "Hero Area",
            Description = "Area for hero blocks only",
            GroupName = "Header",
            Order = 20)]
        [AllowedTypes(typeof(HeroBlock))]
        public virtual ContentArea? HeroArea { get; set; }

        [Display(
            Name = "Top content area",
            Description = "Content area above main content",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [AllowedTypes(typeof(HeroBlock), typeof(RichTextBlock), typeof(CTABlock))]
        public virtual ContentArea? TopContentArea { get; set; }

        [Display(
            Name = "Main content area",
            Description = "Main content area for blocks",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        [AllowedTypes(typeof(HeroBlock), typeof(RichTextBlock), typeof(CTABlock))]
        public virtual ContentArea? MainContentArea { get; set; }
    }
}