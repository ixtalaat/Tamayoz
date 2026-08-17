using Microsoft.AspNetCore.Mvc;
using Tamayoz.Services;
using Tamayoz.ViewModels;

namespace Tamayoz.Controllers;

public class RequestsController(IServiceCatalogService catalog, IRequestManagementService requests) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(int serviceId)
    {
        var service = await catalog.GetActiveByIdAsync(serviceId);
        if (service is null)
        {
            return NotFound();
        }

        return View(new ServiceRequestViewModel
        {
            ServiceId = service.Id,
            ServiceName = service.Name
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var service = await catalog.GetActiveByIdAsync(model.ServiceId);
            model.ServiceName = service?.Name;
            return View(model);
        }

        var isCreated = await requests.CreateAsync(model);
        if (isCreated)
        {
            return RedirectToAction(nameof(Confirmation));
        }

        ModelState.AddModelError(nameof(model.ServiceId), "الخدمة غير متاحة حاليًا.");
        var activeService = await catalog.GetActiveByIdAsync(model.ServiceId);
        model.ServiceName = activeService?.Name;
        return View(model);
    }

    [HttpGet]
    public IActionResult Confirmation()
    {
        return View();
    }
}

