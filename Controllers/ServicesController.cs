using Microsoft.AspNetCore.Mvc;
using Tamayoz.Services;

namespace Tamayoz.Controllers;

public class ServicesController(IServiceCatalogService services) : Controller
{
    public async Task<IActionResult> Index()
    {
        var activeServices = await services.GetActiveAsync();
        return View(activeServices);
    }

    public async Task<IActionResult> Details(int id)
    {
        var service = await services.GetActiveByIdAsync(id);
        if (service is null)
        {
            return NotFound();
        }

        return View(service);
    }
}

