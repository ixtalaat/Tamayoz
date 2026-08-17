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

