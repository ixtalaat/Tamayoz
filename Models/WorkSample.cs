using System.ComponentModel.DataAnnotations;

namespace Tamayoz.Models;

public class WorkSample
{
    public int Id { get; set; }

    [Required(ErrorMessage = "عنوان النموذج أو العينة مطلوب")]
    [StringLength(150, ErrorMessage = "العنوان لا يجب أن يتجاوز 150 حرفًا")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "التصنيف أو القسم مطلوب")]
    [StringLength(80, ErrorMessage = "التصنيف لا يتجاوز 80 حرفًا")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "الوصف ومحتويات العينة مطلوبة")]
    [StringLength(1000, ErrorMessage = "الوصف لا يتجاوز 1000 حرف")]
    public string Description { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [StringLength(500)]
    public string? FileUrl { get; set; }

    [StringLength(50)]
    public string FileType { get; set; } = "PDF";

    [StringLength(80)]
    public string? PagesOrSlidesCount { get; set; }

    [StringLength(80)]
    public string? CitationStyle { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
