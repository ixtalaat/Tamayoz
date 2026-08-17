using Microsoft.AspNetCore.Mvc;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Controllers;
public class ContactController(ApplicationDbContext db) : Controller
{
    public IActionResult Index() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactMessage model)
    {
        if (!ModelState.IsValid) return View(model);
        db.ContactMessages.Add(model); await db.SaveChangesAsync();
        TempData["Success"] = "تم إرسال رسالتك بنجاح. سنتواصل معك قريبًا.";
        return RedirectToAction(nameof(Index));
    }
}
