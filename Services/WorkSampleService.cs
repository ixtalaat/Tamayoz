using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Services;

public class WorkSampleService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) : IWorkSampleService
{
    public async Task<IReadOnlyList<WorkSample>> GetActiveSamplesAsync(string? category = null)
    {
        var query = db.WorkSamples
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ThenByDescending(s => s.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category) && category != "الكل")
        {
            query = query.Where(s => s.Category == category.Trim());
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<WorkSample>> GetAllForAdminAsync()
    {
        return await db.WorkSamples
            .OrderBy(s => s.DisplayOrder)
            .ThenByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<WorkSample?> GetByIdAsync(int id)
    {
        return await db.WorkSamples.FindAsync(id);
    }

    public async Task<bool> CreateAsync(WorkSample sample, IFormFile? thumbnailFile, IFormFile? sampleFile)
    {
        if (thumbnailFile is not null && thumbnailFile.Length > 0)
        {
            var savedThumb = await SaveFileAsync(thumbnailFile, "sample-thumb");
            if (savedThumb is not null)
            {
                sample.ThumbnailUrl = savedThumb;
            }
        }

        if (sampleFile is not null && sampleFile.Length > 0)
        {
            var savedDoc = await SaveFileAsync(sampleFile, "sample-doc");
            if (savedDoc is not null)
            {
                sample.FileUrl = savedDoc;
            }
        }

        sample.CreatedAt = DateTime.UtcNow;
        db.WorkSamples.Add(sample);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(WorkSample sample, IFormFile? thumbnailFile, IFormFile? sampleFile)
    {
        var existing = await db.WorkSamples.FindAsync(sample.Id);
        if (existing is null)
        {
            return false;
        }

        existing.Title = sample.Title;
        existing.Category = sample.Category;
        existing.Description = sample.Description;
        existing.FileType = sample.FileType;
        existing.PagesOrSlidesCount = sample.PagesOrSlidesCount;
        existing.CitationStyle = sample.CitationStyle;
        existing.IsActive = sample.IsActive;
        existing.DisplayOrder = sample.DisplayOrder;

        if (thumbnailFile is not null && thumbnailFile.Length > 0)
        {
            var savedThumb = await SaveFileAsync(thumbnailFile, "sample-thumb");
            if (savedThumb is not null)
            {
                existing.ThumbnailUrl = savedThumb;
            }
        }
        else if (!string.IsNullOrWhiteSpace(sample.ThumbnailUrl))
        {
            existing.ThumbnailUrl = sample.ThumbnailUrl;
        }

        if (sampleFile is not null && sampleFile.Length > 0)
        {
            var savedDoc = await SaveFileAsync(sampleFile, "sample-doc");
            if (savedDoc is not null)
            {
                existing.FileUrl = savedDoc;
            }
        }
        else if (!string.IsNullOrWhiteSpace(sample.FileUrl))
        {
            existing.FileUrl = sample.FileUrl;
        }

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await db.WorkSamples.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        db.WorkSamples.Remove(item);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        var item = await db.WorkSamples.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        item.IsActive = !item.IsActive;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<IReadOnlyList<string>> GetCategoriesAsync()
    {
        return await db.WorkSamples
            .Where(s => s.IsActive)
            .Select(s => s.Category)
            .Distinct()
            .ToListAsync();
    }

    private async Task<string?> SaveFileAsync(IFormFile file, string prefix)
    {
        var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "samples");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var uniqueFileName = $"{prefix}-{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/samples/{uniqueFileName}";
    }
}
