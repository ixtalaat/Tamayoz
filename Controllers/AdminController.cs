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
    ITestimonialService testimonials,
    IWorkSampleService workSamples,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IWebHostEnvironment webHostEnvironment) : Controller
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
    public async Task<IActionResult> CreateService(Service model, IFormFile? imageFile)
    {
        if (imageFile is not null && imageFile.Length > 0)
        {
            var savedPath = await SaveServiceImageAsync(imageFile);
            if (savedPath is not null)
            {
                model.ImageUrl = savedPath;
            }
        }

        if (string.IsNullOrWhiteSpace(model.ImageUrl))
        {
            model.ImageUrl = null;
            ModelState.Remove(nameof(model.ImageUrl));
        }

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
    public async Task<IActionResult> EditService(int id, Service model, IFormFile? imageFile)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (imageFile is not null && imageFile.Length > 0)
        {
            var savedPath = await SaveServiceImageAsync(imageFile);
            if (savedPath is not null)
            {
                model.ImageUrl = savedPath;
            }
        }

        if (string.IsNullOrWhiteSpace(model.ImageUrl))
        {
            model.ImageUrl = null;
            ModelState.Remove(nameof(model.ImageUrl));
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

    private async Task<string?> SaveServiceImageAsync(IFormFile? imageFile)
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            return null;
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg" };
        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return null;
        }

        var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "services");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"service-{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return $"/images/services/{uniqueFileName}";
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var isDeleted = await requests.DeleteAsync(id);
        if (isDeleted)
        {
            TempData["Success"] = "تم حذف طلب الخدمة بنجاح.";
        }
        else
        {
            TempData["Error"] = "لم يتم العثور على الطلب المراد حذفه.";
        }

        return RedirectToAction(nameof(Requests));
    }

    public async Task<IActionResult> Messages()
    {
        var messagesList = await messages.GetAllAsync();
        return View(messagesList);
    }

    public async Task<IActionResult> Testimonials(bool? approved)
    {
        ViewBag.ApprovedFilter = approved;
        var list = await testimonials.GetAllForAdminAsync(approved);
        return View(list);
    }

    public async Task<IActionResult> Samples()
    {
        var list = await workSamples.GetAllForAdminAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult CreateSample()
    {
        return View(new WorkSample());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSample(WorkSample model, IFormFile? thumbnailFile, IFormFile? sampleFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await workSamples.CreateAsync(model, thumbnailFile, sampleFile);
        TempData["Success"] = "تمت إضافة نموذج العمل بنجاح إلى المعرض.";
        return RedirectToAction(nameof(Samples));
    }

    [HttpGet]
    public async Task<IActionResult> EditSample(int id)
    {
        var sample = await workSamples.GetByIdAsync(id);
        if (sample is null)
        {
            return NotFound();
        }

        return View(sample);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSample(int id, WorkSample model, IFormFile? thumbnailFile, IFormFile? sampleFile)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var isUpdated = await workSamples.UpdateAsync(model, thumbnailFile, sampleFile);
        if (!isUpdated)
        {
            return NotFound();
        }

        TempData["Success"] = "تم تحديث نموذج العمل بنجاح.";
        return RedirectToAction(nameof(Samples));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSample(int id)
    {
        var isDeleted = await workSamples.DeleteAsync(id);
        if (isDeleted)
        {
            TempData["Success"] = "تم حذف النموذج بنجاح من المعرض.";
        }
        return RedirectToAction(nameof(Samples));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleSampleActive(int id)
    {
        await workSamples.ToggleActiveAsync(id);
        TempData["Success"] = "تم تغيير حالة ظهور النموذج في المعرض.";
        return RedirectToAction(nameof(Samples));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveTestimonial(int id)
    {
        var isApproved = await testimonials.ApproveAsync(id);
        if (isApproved)
        {
            TempData["Success"] = "تم اعتماد ونشر التقييم بنجاح ليظهر في واجهة الموقع.";
        }
        return RedirectToAction(nameof(Testimonials));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTestimonial(int id)
    {
        var isDeleted = await testimonials.DeleteAsync(id);
        if (isDeleted)
        {
            TempData["Success"] = "تم حذف التقييم بنجاح.";
        }
        return RedirectToAction(nameof(Testimonials));
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


