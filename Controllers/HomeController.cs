using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tamayoz.Models;
using Tamayoz.Services;

namespace Tamayoz.Controllers;
public class HomeController(IServiceCatalogService services) : Controller
{
    public async Task<IActionResult> Index() => View(await services.GetActiveAsync(3));
    public IActionResult Privacy() => View();
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
