using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Controllers;
public class RequestsController(ApplicationDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(int serviceId)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == serviceId && s.IsActive);
        return service is null ? NotFound() : View(new ServiceRequestViewModel { ServiceId = service.Id, ServiceName = service.Name });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequestViewModel model)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == model.ServiceId && s.IsActive);
        if (service is null) ModelState.AddModelError(nameof(model.ServiceId), "الخدمة غير متاحة حاليًا.");
        if (!ModelState.IsValid) { model.ServiceName = service?.Name; return View(model); }
        db.ServiceRequests.Add(new ServiceRequest { ServiceId = model.ServiceId, StudentName = model.StudentName, StudentEmail = model.StudentEmail, StudentPhone = model.StudentPhone, Message = model.Message, PreferredContactMethod = model.PreferredContactMethod });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Confirmation));
    }
    public IActionResult Confirmation() => View();
}
