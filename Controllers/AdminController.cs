using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamayoz.Models;
using Tamayoz.Services;

namespace Tamayoz.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IAdminDashboardService dashboard, IServiceCatalogService catalog, IRequestManagementService requests) : Controller
{
    public async Task<IActionResult> Index() => View(await dashboard.GetStatisticsAsync());
    public async Task<IActionResult> Requests(RequestStatus? status) => View(await requests.GetAllAsync(status));
    public async Task<IActionResult> Services() => View(await catalog.GetAllAsync());
    [HttpGet] public IActionResult CreateService() => View(new Service());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateService(Service model)
    {
        if (!ModelState.IsValid) return View(model);
        await catalog.CreateAsync(model); TempData["Success"] = "تمت إضافة الخدمة.";
        return RedirectToAction(nameof(Services));
    }
    public async Task<IActionResult> EditService(int id) => await catalog.GetByIdAsync(id) is { } service ? View(service) : NotFound();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditService(int id, Service model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(model);
        if (!await catalog.UpdateAsync(model)) return NotFound();
        TempData["Success"] = "تم تحديث الخدمة."; return RedirectToAction(nameof(Services));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteService(int id)
    {
        await catalog.RemoveOrDeactivateAsync(id); TempData["Success"] = "تمت إزالة الخدمة أو إيقافها لحماية الطلبات السابقة.";
        return RedirectToAction(nameof(Services));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRequestStatus(int id, RequestStatus status)
    {
        if (!await requests.UpdateStatusAsync(id, status)) return NotFound();
        return RedirectToAction(nameof(Requests));
    }
}
