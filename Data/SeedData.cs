using Microsoft.EntityFrameworkCore;
using Tamayoz.Models;

namespace Tamayoz.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Services.AnyAsync())
        {
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

        if (!await db.Testimonials.AnyAsync())
        {
            db.Testimonials.AddRange(
                new Testimonial
                {
                    StudentName = "م. سارة كمال",
                    AcademicDegree = "طالبة ماجستير - هندسة حاسبات",
                    ServiceName = "مشروع تخرج ذكاء اصطناعي",
                    Rating = 5,
                    Comment = "تجربة ممتازة جداً! تم تنفيذ نموذج التعلم العميق وشرح تفاصيل الأكواد بدقة عالية، مما ساعدني في مناقشة مشروعي والحصول على تقدير امتياز.",
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    ApprovedAt = DateTime.UtcNow.AddDays(-14)
                },
                new Testimonial
                {
                    StudentName = "أحمد عبد الرحمن",
                    AcademicDegree = "بكالوريوس تجارة وإدارة أعمال",
                    ServiceName = "بحث دراسة جدوى وتسويق",
                    Rating = 5,
                    Comment = "دقة متناهية في التوثيق والمراجع وخلو تام من الاقتباس. تم تسليم العمل قبل الموعد المحدد مع دعم كامل في الملاحظات.",
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    ApprovedAt = DateTime.UtcNow.AddDays(-9)
                },
                new Testimonial
                {
                    StudentName = "نور الهدى إبراهيم",
                    AcademicDegree = "دراسات عليا - كلية التربية",
                    ServiceName = "تحليل إحصائي SPSS وبحث أكاديمي",
                    Rating = 5,
                    Comment = "التعامل راقٍ جداً والمشرف الأكاديمي كان متعاوناً إلى أقصى درجة في تعديل الجداول وتفسير النتائج الإحصائية.",
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    ApprovedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Testimonial
                {
                    StudentName = "عمر خالد",
                    AcademicDegree = "كلية الهندسة المعمارية",
                    ServiceName = "مخططات AutoCAD وعرض تقديمي",
                    Rating = 5,
                    Comment = "شغل محترف وسريع أنقذني في موعد التسليم النهائي. التصاميم كانت متناسقة جداً والبرزنتيشن أبهر الدكتور.",
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    ApprovedAt = DateTime.UtcNow.AddDays(-3)
                }
            );

            await db.SaveChangesAsync();
        }

        if (!await db.WorkSamples.AnyAsync())
        {
            db.WorkSamples.AddRange(
                new WorkSample
                {
                    Title = "نموذج بحث أكاديمي وتوثيق مراجع (دراسة تسويقية)",
                    Category = "أبحاث علمية",
                    Description = "عينة بحثية توضح أسلوب الصياغة الأكاديمية الدقيقة، توثيق المراجع بنظام APA 7th Edition، التنسيق المنهجي، وقائمة المصادر الحديثة.",
                    ThumbnailUrl = "https://images.unsplash.com/photo-1456513080510-7bf3a84b82f8?auto=format&fit=crop&w=800&q=80",
                    FileType = "PDF / Word",
                    PagesOrSlidesCount = "24 صفحة",
                    CitationStyle = "APA 7th",
                    IsActive = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new WorkSample
                {
                    Title = "مشروع تخرج نظام تصنيف ذكي بالتعلم العميق (AI & Python)",
                    Category = "حلول برمجية",
                    Description = "عينة من هيكلية الكود البرمجي الموثق، ملفات الشرح التفصيلي للـ Architecture، والـ Data Pipeline مع نتائج الفحص ودقة النموذج.",
                    ThumbnailUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=800&q=80",
                    FileType = "Python / ZIP",
                    PagesOrSlidesCount = "كود + تقرير 35 صفحة",
                    CitationStyle = "IEEE",
                    IsActive = true,
                    DisplayOrder = 2,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new WorkSample
                {
                    Title = "عرض تقديمي تفاعلي وتصميم بصري لمناقشة تخرج (Master Defense)",
                    Category = "عروض تقديمية",
                    Description = "تصميم PowerPoint أنيق واحترافي يعتمد التسلسل البصري للأفكار، الإنفوجرافيك، والجداول التوضيحية الجاهزة للمناقشة المباشرة.",
                    ThumbnailUrl = "https://images.unsplash.com/photo-1557804506-669a67965ba0?auto=format&fit=crop&w=800&q=80",
                    FileType = "PowerPoint / PDF",
                    PagesOrSlidesCount = "22 شريحة تفاعلية",
                    CitationStyle = "Harvard",
                    IsActive = true,
                    DisplayOrder = 3,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new WorkSample
                {
                    Title = "مخططات هندسية معمارية ورسومات تنفيذية AutoCAD",
                    Category = "تصميم هندسي",
                    Description = "نموذج للمساقط الأفقية والواجهات والتفاصيل الإنشائية التنفيذية مع جداول الكميات والرموز الهندسية المعتمدة.",
                    ThumbnailUrl = "https://images.unsplash.com/photo-1581092160607-ee22621dd758?auto=format&fit=crop&w=800&q=80",
                    FileType = "AutoCAD / DWG / PDF",
                    PagesOrSlidesCount = "8 لوحات تنفيذية",
                    CitationStyle = "معايير هندسية",
                    IsActive = true,
                    DisplayOrder = 4,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            );

            await db.SaveChangesAsync();
        }
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

