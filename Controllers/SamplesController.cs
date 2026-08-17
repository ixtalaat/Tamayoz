using Microsoft.AspNetCore.Mvc;
using Tamayoz.Services;

namespace Tamayoz.Controllers;

public class SamplesController(IWorkSampleService samplesService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? category)
    {
        ViewBag.CurrentCategory = category ?? "الكل";
        ViewBag.Categories = await samplesService.GetCategoriesAsync();
        var samples = await samplesService.GetActiveSamplesAsync(category);
        return View(samples);
    }
}
