using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Tamayoz.ViewModels;

public class ServiceRequestViewModel
{
    [Required]
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(120, ErrorMessage = "الاسم لا يجب أن يتجاوز 120 حرفًا")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "أدخل بريدًا إلكترونيًا صحيحًا")]
    public string StudentEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "أدخل رقم هاتف صحيحًا")]
    public string StudentPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "يرجى كتابة تفاصيل طلبك")]
    [StringLength(2000, ErrorMessage = "الرسالة لا يجب أن تتجاوز 2000 حرف")]
    public string Message { get; set; } = string.Empty;

    public string? PreferredContactMethod { get; set; }

    public IFormFile? AttachmentFile { get; set; }
}

