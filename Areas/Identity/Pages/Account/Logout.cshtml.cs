using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tamayoz.Areas.Identity.Pages.Account;

public class LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger) : PageModel
{
    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("Admin user logged out successfully.");

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnGet(string? returnUrl = null)
    {
        if (signInManager.IsSignedIn(User))
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("Admin user logged out via GET.");
        }

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }

        return Page();
    }
}
