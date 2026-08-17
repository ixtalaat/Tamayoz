using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tamayoz.Models;
using Tamayoz.Services;
using Tamayoz.ViewModels;

namespace Tamayoz.Controllers;

[Authorize(Policy = "AdminOnly")]
public class AdminController(
    IAdminDashboardService dashboard,
    IServiceCatalogService catalog,
    IRequestManagementService requests,
    IContactMessageService messages,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : Controller
{

    public async Task<IActionResult> Index()
    {
        var dashboardData = await dashboard.GetAsync();
        return View(dashboardData);
    }

    public async Task<IActionResult> Requests(RequestStatus? status)
    {
        var requestsList = await requests.GetAllAsync(status);
        return View(requestsList);
    }

    public async Task<IActionResult> Services()
    {
        var servicesList = await catalog.GetAllAsync();
        return View(servicesList);
    }

    [HttpGet]
    public IActionResult CreateService()
    {
        return View(new Service());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateService(Service model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await catalog.CreateAsync(model);
        TempData["Success"] = "تمت إضافة الخدمة بنجاح.";
        return RedirectToAction(nameof(Services));
    }

    [HttpGet]
    public async Task<IActionResult> EditService(int id)
    {
        var service = await catalog.GetByIdAsync(id);
        if (service is null)
        {
            return NotFound();
        }

        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditService(int id, Service model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var isUpdated = await catalog.UpdateAsync(model);
        if (!isUpdated)
        {
            return NotFound();
        }

        TempData["Success"] = "تم تحديث الخدمة بنجاح.";
        return RedirectToAction(nameof(Services));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteService(int id)
    {
        await catalog.RemoveOrDeactivateAsync(id);
        TempData["Success"] = "تمت إزالة الخدمة أو إيقافها لحماية الطلبات السابقة.";
        return RedirectToAction(nameof(Services));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRequestStatus(int id, RequestStatus status)
    {
        var isUpdated = await requests.UpdateStatusAsync(id, status);
        if (!isUpdated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Requests));
    }

    public async Task<IActionResult> Messages()
    {
        var messagesList = await messages.GetAllAsync();
        return View(messagesList);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkMessageRead(int id)
    {
        var isUpdated = await messages.MarkReadAsync(id);
        if (!isUpdated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Messages));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        await messages.DeleteAsync(id);
        return RedirectToAction(nameof(Messages));
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var changePasswordResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                var localizedDescription = error.Code switch
                {
                    "PasswordMismatch" => "كلمة المرور الحالية غير صحيحة.",
                    "PasswordTooShort" => "كلمة المرور قصيرة جدًا. يجب ألا تقل عن 6 أحرف.",
                    "PasswordRequiresNonAlphanumeric" => "يجب أن تحتوي كلمة المرور على رمز خاص واحد على الأقل.",
                    "PasswordRequiresDigit" => "يجب أن تحتوي كلمة المرور على رقم واحد على الأقل ('0'-'9').",
                    "PasswordRequiresLower" => "يجب أن تحتوي كلمة المرور على حرف صغير واحد على الأقل ('a'-'z').",
                    "PasswordRequiresUpper" => "يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل ('A'-'Z').",
                    _ => error.Description
                };
                ModelState.AddModelError(string.Empty, localizedDescription);
            }

            return View(model);
        }

        await signInManager.RefreshSignInAsync(user);
        TempData["Success"] = "تم تغيير كلمة المرور بنجاح.";
        return RedirectToAction(nameof(ChangePassword));
    }
}


