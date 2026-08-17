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

    [Route("Home/Error/{statusCode:int?}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode = null)
    {
        var code = statusCode ?? (Response.StatusCode != 200 ? Response.StatusCode : 500);
        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = code
        };

        switch (code)
        {
            case 404:
                model.Title = "الصفحة المطلوبة غير موجودة (404)";
                model.Message = "عذرًا، يبدو أن الرابط الذي تحاول الوصول إليه غير صحيح أو تم نقله أو حذفه.";
                break;
            case 403:
                model.Title = "غير مصرح بالوصول (403)";
                model.Message = "عذرًا، ليس لديك الصلاحية الكافية للوصول إلى هذا القسم. يرجى التأكد من تسجيل الدخول بحساب المشرف.";
                break;
            case 400:
                model.Title = "طلب غير صالح (400)";
                model.Message = "تعذر على الخادم معالجة هذا الطلب بسبب صيغة غير صالحة.";
                break;
            default:
                model.Title = "حدث خطأ غير متوقع في النظام";
                model.Message = "نعتذر عن هذا الخطأ المؤقت. فريق الدعم الفني تم إشعاره تلقائيًا ونعمل على معالجة المشكلة فورًا.";
                break;
        }

        return View(model);
    }

}

