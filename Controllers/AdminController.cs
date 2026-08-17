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
