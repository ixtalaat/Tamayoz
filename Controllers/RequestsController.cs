using Microsoft.AspNetCore.Mvc;
using Tamayoz.Models;
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

        var trackingCode = await requests.CreateAsync(model);
        if (trackingCode is not null)
        {
            TempData["Success"] = "تم استلام طلبك بنجاح! كود تتبع طلبك هو " + trackingCode;
            return RedirectToAction(nameof(Confirmation), new { code = trackingCode });
        }

        ModelState.AddModelError(nameof(model.ServiceId), "الخدمة غير متاحة حاليًا.");
        var activeService = await catalog.GetActiveByIdAsync(model.ServiceId);
        model.ServiceName = activeService?.Name;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return View(model: (ServiceRequest?)null);
        }

        var request = await requests.GetByTrackingCodeAsync(code);
        return View(model: request);
    }

    [HttpGet]
    public async Task<IActionResult> Track(string? code, string? phone)
    {
        ViewBag.SearchCode = code;
        ViewBag.SearchPhone = phone;
        ViewBag.HasSearched = !string.IsNullOrWhiteSpace(code) || !string.IsNullOrWhiteSpace(phone);

        if (!string.IsNullOrWhiteSpace(code))
        {
            var singleRequest = await requests.GetByTrackingCodeAsync(code);
            if (singleRequest is not null)
            {
                ViewBag.RequestsList = new List<ServiceRequest> { singleRequest };
                return View();
            }
            ViewBag.NotFound = true;
            return View();
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            var list = await requests.GetByPhoneAsync(phone);
            if (list.Count > 0)
            {
                ViewBag.RequestsList = list;
                return View();
            }
            ViewBag.NotFound = true;
            return View();
        }

        return View();
    }
}

