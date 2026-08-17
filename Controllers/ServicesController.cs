using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;

namespace Tamayoz.Controllers;
public class ServicesController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Services.Where(s => s.IsActive).OrderByDescending(s => s.CreatedAt).ToListAsync());
    public async Task<IActionResult> Details(int id)
    {
        var service = await db.Services.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        return service is null ? NotFound() : View(service);
    }
}
