using Microsoft.EntityFrameworkCore;
using Tamayoz.Models;

namespace Tamayoz.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Services.AnyAsync())
        {
            return;
        }

        db.Services.AddRange(
            Create(
                name: "الخدمات الأكاديمية والأبحاث",
                shortDescription: "أبحاث، تقارير، واجبات، تلخيصات ودراسات حالة بأعلى المعايير الأكاديمية.",
                description: "نساعدك في إعداد الأبحاث والتقارير والواجبات والتلخيصات والمقالات ودراسات الحالة، مع تنظيم احترافي وتوثيق علمي دقيق للمراجع وتسليم في الموعد المتفق عليه."
            ),
            Create(
                name: "الأسايمنت ومشاريع التخرج",
                shortDescription: "Assignments ومشاريع تخرج جامعية بجودة عالية لمختلف الكليات.",
                description: "تنفيذ ومراجعة الأسايمنت والبروجكت والتكاليف ومشاريع التخرج لمختلف التخصصات، مع متابعة واضحة لكل مرحلة من مراحل المشروع حتى تسليمه."
            ),
            Create(
                name: "العروض التقديمية والبرزنتيشن",
                shortDescription: "عروض PowerPoint احترافية وتصاميم بصرية مميزة وجذابة للمناقشات.",
                description: "إعداد وتصميم عروض باوربوينت تفاعلية ومنسقة باحترافية، مناسبة للمشاريع الجامعية والمناقشات والعروض الأكاديمية، مع بطاقات شرح وتلخيص."
            ),
            Create(
                name: "الحلول والخدمات البرمجية",
                shortDescription: "مشاريع وتكاليف برمجية في Java وC# وC++ وPython والذكاء الاصطناعي مع شرح الكود.",
                description: "تنفيذ متكامل لمشاريع وواجبات البرمجة بمختلف لغات البرمجة (Python, Java, C#, Web, AI)، مع توفير شروحات مبسطة لطريقة عمل الكود والمناقشة."
            ),
            Create(
                name: "رسائل الماجستير والدكتوراه",
                shortDescription: "دعم بحثي وإحصائي وتدقيق شامل لرسائل الدراسات العليا.",
                description: "مساعدة بحثية متخصصة لطلبة الدراسات العليا تشمل إعداد خطة البحث (Proposal)، الإطار النظري، التحليل الإحصائي، والتدقيق اللغوي الشامل."
            ),
            Create(
                name: "التصميم الهندسي والمونتاج",
                shortDescription: "AutoCAD و3D Max وPhotoshop وتصاميم ومخططات هندسية متكاملة.",
                description: "خدمات التصاميم الهندسية والمخططات والمونتاج باستخدام AutoCAD و3D Max وPhotoshop، تشمل الرسومات المعمارية والإنشائية المطلوبة للمشاريع."
            )
        );

        await db.SaveChangesAsync();
    }

    private static Service Create(string name, string shortDescription, string description)
    {
        return new Service
        {
            Name = name,
            ShortDescription = shortDescription,
            Description = description,
            EstimatedDuration = "يُحدّد عند مراجعة الطلب",
            IsActive = true
        };
    }
}

