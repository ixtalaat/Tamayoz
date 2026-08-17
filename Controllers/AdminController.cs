using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(new Dictionary<string, int>
    {
        ["إجمالي الخدمات"] = await db.Services.CountAsync(),
        ["الخدمات النشطة"] = await db.Services.CountAsync(s => s.IsActive),
        ["طلبات معلقة"] = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.Pending),
        ["طلبات قيد التنفيذ"] = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.InProgress),
        ["رسائل جديدة"] = await db.ContactMessages.CountAsync(m => !m.IsRead)
    });

    public async Task<IActionResult> Requests(RequestStatus? status)
    {
        var query = db.ServiceRequests.Include(r => r.Service).AsQueryable();
        if (status.HasValue) query = query.Where(r => r.Status == status);
        return View(await query.OrderByDescending(r => r.CreatedAt).ToListAsync());
    }

    public async Task<IActionResult> Services() => View(await db.Services.OrderByDescending(s => s.CreatedAt).ToListAsync());

    [HttpGet]
    public IActionResult CreateService() => View(new Service());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateService(Service model)
    {
        if (!ModelState.IsValid) return View(model);
        model.CreatedAt = DateTime.UtcNow;
        db.Services.Add(model); await db.SaveChangesAsync();
        TempData["Success"] = "تمت إضافة الخدمة.";
        return RedirectToAction(nameof(Services));
    }

    [HttpGet]
    public async Task<IActionResult> EditService(int id)
    {
        var service = await db.Services.FindAsync(id);
        return service is null ? NotFound() : View(service);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditService(int id, Service model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(model);
        var service = await db.Services.FindAsync(id);
        if (service is null) return NotFound();
        service.Name = model.Name; service.ShortDescription = model.ShortDescription; service.Description = model.Description;
        service.Price = model.Price; service.EstimatedDuration = model.EstimatedDuration; service.ImageUrl = model.ImageUrl;
        service.IsActive = model.IsActive; service.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        TempData["Success"] = "تم تحديث الخدمة.";
        return RedirectToAction(nameof(Services));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await db.Services.Include(s => s.Requests).FirstOrDefaultAsync(s => s.Id == id);
        if (service is not null && !service.Requests.Any()) db.Services.Remove(service);
        else if (service is not null) { service.IsActive = false; service.UpdatedAt = DateTime.UtcNow; }
        await db.SaveChangesAsync();
        TempData["Success"] = "تمت إزالة الخدمة أو إيقافها لحماية الطلبات السابقة.";
        return RedirectToAction(nameof(Services));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRequestStatus(int id, RequestStatus status)
    {
        var request = await db.ServiceRequests.FindAsync(id);
        if (request is null) return NotFound();
        request.Status = status; request.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Requests));
    }
}
