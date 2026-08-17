using Microsoft.EntityFrameworkCore;
using Tamayoz.Models;

namespace Tamayoz.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        await db.Database.MigrateAsync();
        if (await db.Services.AnyAsync()) return;
        db.Services.AddRange(
            Create("الخدمات الأكاديمية", "أبحاث، تقارير، واجبات، تلخيصات ودراسات حالة.", "نساعدك في إعداد الأبحاث والتقارير والواجبات والتلخيصات والمقالات ودراسات الحالة، مع تنظيم احترافي وتسليم في الموعد المتفق عليه."),
            Create("الأسايمنت والمشاريع", "Assignments، Projects وتكاليف جامعية بجودة عالية.", "تنفيذ ومراجعة الأسايمنت والبروجكت والتكاليف والمشاريع الدراسية لمختلف التخصصات، مع متابعة واضحة لكل طلب."),
            Create("العروض التقديمية", "PowerPoint وبرزنتيشن احترافي وواضح.", "إعداد عروض باوربوينت وبرزنتيشن منظمة وجذابة، مناسبة للمشاريع الجامعية والمناقشات والعروض الأكاديمية."),
            Create("خدمات البرمجة", "Java، C#، C++ وPython مع شرح ودعم.", "مساعدة في واجبات ومشاريع البرمجة باستخدام Java وC# وC++ وPython، بالإضافة إلى شروحات عملية للمفاهيم البرمجية."),
            Create("رسائل الدراسات العليا", "دعم لرسائل الماجستير والدكتوراه.", "مساعدة منظمة في إعداد رسائل الماجستير والدكتوراه من حيث البحث والتنسيق والمراجعة، بما يناسب احتياجك الأكاديمي."),
            Create("التصميم الهندسي والمونتاج", "AutoCAD، 3D Max، Photoshop وتصاميم هندسية.", "خدمات التصاميم الهندسية والمونتاج باستخدام AutoCAD و3D Max وPhotoshop، بما يشمل الرسومات والتصميمات المطلوبة للمشاريع.")
        );
        await db.SaveChangesAsync();
    }

    private static Service Create(string name, string shortDescription, string description) => new() { Name = name, ShortDescription = shortDescription, Description = description, EstimatedDuration = "يُحدّد عند مراجعة الطلب", IsActive = true };
}
