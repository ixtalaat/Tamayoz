using System.ComponentModel.DataAnnotations;

namespace Tamayoz.Models;

public class Testimonial
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم الطالب أو التخصص مطلوب")]
    [StringLength(100, ErrorMessage = "الاسم لا يتجاوز 100 حرف")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "الجامعة أو الدرجة الأكاديمية مطلوبة")]
    [StringLength(150, ErrorMessage = "الدرجة الأكاديمية لا تتجاوز 150 حرفًا")]
    public string AcademicDegree { get; set; } = string.Empty;

    [StringLength(150)]
    public string? ServiceName { get; set; }

    [Range(1, 5, ErrorMessage = "التقييم يجب أن يكون بين 1 و 5")]
    public int Rating { get; set; } = 5;

    [Required(ErrorMessage = "نص الرأي أو التقييم مطلوب")]
    [StringLength(1000, ErrorMessage = "نص التقييم لا يتجاوز 1000 حرف")]
    public string Comment { get; set; } = string.Empty;

    public bool IsApproved { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ApprovedAt { get; set; }
}
