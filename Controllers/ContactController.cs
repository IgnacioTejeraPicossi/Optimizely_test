using Microsoft.AspNetCore.Mvc;

namespace OptiDemoCms.Controllers;

/// <summary>
/// Handles the contact form submission from the Contact Me page.
/// </summary>
[Route("contact")]
public class ContactController : Controller
{
    [HttpPost("send")]
    [ValidateAntiForgeryToken]
    public IActionResult SendMessage(string senderName, string senderEmail, string message)
    {
        // Demo: no email sent; just redirect with success message.
        // TODO: add email sending (e.g. IEmailSender) or save to DB if needed.
        TempData["ContactMessageSent"] = true;
        return Redirect("/contact-me/");
    }
}
