using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tamayoz.Models;
using Tamayoz.Services;

namespace Tamayoz.Controllers;

public class HomeController(IServiceCatalogService services) : Controller
{
    public async Task<IActionResult> Index()
    {
        var featuredServices = await services.GetActiveAsync(6);
        return View(featuredServices);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}

