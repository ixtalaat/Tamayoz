using Microsoft.AspNetCore.Mvc;
using Tamayoz.Services;
using Tamayoz.ViewModels;

namespace Tamayoz.Controllers;
public class RequestsController(IServiceCatalogService catalog, IRequestManagementService requests) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(int serviceId) => await catalog.GetActiveByIdAsync(serviceId) is { } service
        ? View(new ServiceRequestViewModel { ServiceId = service.Id, ServiceName = service.Name }) : NotFound();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequestViewModel model)
    {
        if (!ModelState.IsValid) { model.ServiceName = (await catalog.GetActiveByIdAsync(model.ServiceId))?.Name; return View(model); }
        if (await requests.CreateAsync(model)) return RedirectToAction(nameof(Confirmation));
        ModelState.AddModelError(nameof(model.ServiceId), "الخدمة غير متاحة حاليًا.");
        model.ServiceName = (await catalog.GetActiveByIdAsync(model.ServiceId))?.Name;
        return View(model);
    }
    public IActionResult Confirmation() => View();
}
