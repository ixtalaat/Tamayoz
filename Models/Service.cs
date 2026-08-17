using System.ComponentModel.DataAnnotations;

namespace Tamayoz.Models;

public class Service
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم الخدمة مطلوب")]
    [StringLength(120, ErrorMessage = "اسم الخدمة لا يجب أن يتجاوز 120 حرفًا")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "الوصف المختصر مطلوب")]
    [StringLength(280, ErrorMessage = "الوصف المختصر لا يجب أن يتجاوز 280 حرفًا")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "الوصف الكامل مطلوب")]
    [StringLength(3000, ErrorMessage = "الوصف لا يجب أن يتجاوز 3000 حرف")]
    public string Description { get; set; } = string.Empty;

    [Range(0, 999999, ErrorMessage = "السعر يجب أن يكون قيمة موجبة")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "المدة المتوقعة مطلوبة")]
    [StringLength(100, ErrorMessage = "المدة لا يجب أن تتجاوز 100 حرف")]
    public string EstimatedDuration { get; set; } = string.Empty;

    [Url(ErrorMessage = "أدخل رابط صورة صحيح")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<ServiceRequest> Requests { get; set; } = new List<ServiceRequest>();
}

