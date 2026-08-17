using Microsoft.AspNetCore.Mvc;
using Tamayoz.Models;
using Tamayoz.Services;

namespace Tamayoz.Controllers;

public class ContactController(IContactMessageService messages) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactMessage model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await messages.CreateAsync(model);
        TempData["Success"] = "تم إرسال رسالتك بنجاح. سنتواصل معك قريبًا.";
        return RedirectToAction(nameof(Index));
    }
}

