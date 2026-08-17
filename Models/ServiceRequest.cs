using System.ComponentModel.DataAnnotations;

namespace Tamayoz.Models;

public class ServiceRequest
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public Service? Service { get; set; }

    [Required(ErrorMessage = "اسم الطالب مطلوب")]
    [StringLength(120, ErrorMessage = "الاسم لا يجب أن يتجاوز 120 حرفًا")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "أدخل بريدًا إلكترونيًا صحيحًا")]
    [StringLength(160)]
    public string StudentEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "أدخل رقم هاتف صحيح")]
    [StringLength(30)]
    public string StudentPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "تفاصيل الطلب مطلوبة")]
    [StringLength(2000, ErrorMessage = "الرسالة لا يجب أن تتجاوز 2000 حرف")]
    public string Message { get; set; } = string.Empty;

    [StringLength(40)]
    public string? PreferredContactMethod { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

