using EPiServer.Core;
using EPiServer.DataAnnotations;
using OptiDemoCms.Models.Blocks;
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
        [Required(ErrorMessage = "Heading is required")]
        [CultureSpecific]
        public virtual string? Heading { get; set; }

        [Display(
            Name = "Main body",
            Description = "Main body content",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual XhtmlString? MainBody { get; set; }

        [Display(
            Name = "Main content area",
            Description = "Content area for blocks (Hero, Rich Text, CTA)",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [AllowedTypes(typeof(HeroBlock), typeof(RichTextBlock), typeof(CTABlock))]
        public virtual ContentArea? MainContentArea { get; set; }

        [Display(
            Name = "Meta title",
            Description = "SEO title (defaults to heading if empty)",
            GroupName = "SEO",
            Order = 40)]
        [CultureSpecific]
        public virtual string? MetaTitle { get; set; }

        [Display(
            Name = "Meta description",
            Description = "SEO meta description",
            GroupName = "SEO",
            Order = 50)]
        [CultureSpecific]
        public virtual string? MetaDescription { get; set; }

        [Display(
            Name = "Canonical URL",
            Description = "Optional canonical URL for SEO",
            GroupName = "SEO",
            Order = 60)]
        public virtual string? CanonicalUrl { get; set; }
    }
}
