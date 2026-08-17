using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tamayoz.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();
    public string? ReturnUrl { get; set; }
    public class InputModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب"), EmailAddress(ErrorMessage = "أدخل بريدًا إلكترونيًا صحيحًا")] public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "كلمة المرور مطلوبة"), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
    public void OnGet(string? returnUrl = null) => ReturnUrl = returnUrl ?? Url.Content("~/");
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
        if (!ModelState.IsValid) return Page();
        var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded) { logger.LogInformation("User logged in."); return LocalRedirect(ReturnUrl); }
        ModelState.AddModelError(string.Empty, "تعذر تسجيل الدخول. تحقق من البريد الإلكتروني وكلمة المرور.");
        return Page();
    }
}
