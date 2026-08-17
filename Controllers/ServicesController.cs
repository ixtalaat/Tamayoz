using Microsoft.AspNetCore.Mvc;
using Tamayoz.Services;

namespace Tamayoz.Controllers;
public class ServicesController(IServiceCatalogService services) : Controller
{
    public async Task<IActionResult> Index() => View(await services.GetActiveAsync());
    public async Task<IActionResult> Details(int id) => await services.GetActiveByIdAsync(id) is { } service ? View(service) : NotFound();
}
