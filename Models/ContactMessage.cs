using System.ComponentModel.DataAnnotations;

namespace Tamayoz.Models;

public class ContactMessage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(120, ErrorMessage = "الاسم لا يجب أن يتجاوز 120 حرفًا")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "أدخل بريدًا إلكترونيًا صحيحًا")]
    [StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "أدخل رقم هاتف صحيح")]
    [StringLength(30)]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "نص الرسالة مطلوب")]
    [StringLength(2000, ErrorMessage = "الرسالة لا يجب أن تتجاوز 2000 حرف")]
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

