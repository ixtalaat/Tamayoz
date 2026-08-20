using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public class RequestManagementService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) : IRequestManagementService
{
    public async Task<string?> CreateAsync(ServiceRequestViewModel model)
    {
        var isServiceActive = await db.Services.AnyAsync(s => s.Id == model.ServiceId && s.IsActive);
        if (!isServiceActive)
        {
            return null;
        }

        string? attachmentPath = null;
        string? attachmentFileName = null;
        long? attachmentSize = null;

        if (model.AttachmentFile is not null && model.AttachmentFile.Length > 0)
        {
            // Allowed size up to 25 MB
            if (model.AttachmentFile.Length <= 25 * 1024 * 1024)
            {
                var originalName = Path.GetFileName(model.AttachmentFile.FileName);
                var extension = Path.GetExtension(originalName).ToLowerInvariant();

                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", "requests");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var safeUniqueName = $"req-{Guid.NewGuid():N}{extension}";
                var fullFilePath = Path.Combine(uploadsFolder, safeUniqueName);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await model.AttachmentFile.CopyToAsync(stream);
                }

                attachmentPath = $"/uploads/requests/{safeUniqueName}";
                attachmentFileName = originalName;
                attachmentSize = model.AttachmentFile.Length;
            }
        }

        var trackingCode = await GenerateUniqueTrackingCodeAsync();

        var entity = new ServiceRequest
        {
            ServiceId = model.ServiceId,
            TrackingCode = trackingCode,
            StudentName = model.StudentName,
            StudentEmail = model.StudentEmail,
            StudentPhone = model.StudentPhone,
            Message = model.Message,
            PreferredContactMethod = model.PreferredContactMethod,
            AttachmentPath = attachmentPath,
            AttachmentFileName = attachmentFileName,
            AttachmentSize = attachmentSize,
            CreatedAt = DateTime.UtcNow
        };

        db.ServiceRequests.Add(entity);
        await db.SaveChangesAsync();
        return trackingCode;
    }

    public async Task<ServiceRequest?> GetByTrackingCodeAsync(string trackingCode)
    {
        if (string.IsNullOrWhiteSpace(trackingCode))
        {
            return null;
        }

        var normalizedCode = trackingCode.Trim().ToUpperInvariant();
        return await db.ServiceRequests
            .Include(r => r.Service)
            .FirstOrDefaultAsync(r => r.TrackingCode.ToUpper() == normalizedCode);
    }

    public async Task<IReadOnlyList<ServiceRequest>> GetByPhoneAsync(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return Array.Empty<ServiceRequest>();
        }

        var cleanPhone = phone.Trim().Replace(" ", "").Replace("-", "");
        return await db.ServiceRequests
            .Include(r => r.Service)
            .Where(r => r.StudentPhone.Replace(" ", "").Replace("-", "").Contains(cleanPhone))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ServiceRequest>> GetAllAsync(RequestStatus? status)
    {
        var query = db.ServiceRequests
            .Include(r => r.Service)
            .OrderByDescending(r => r.CreatedAt)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(int id, RequestStatus status)
    {
        var request = await db.ServiceRequests.FindAsync(id);
        if (request is null)
        {
            return false;
        }

        request.Status = status;
        request.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var request = await db.ServiceRequests.FindAsync(id);
        if (request is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.AttachmentPath))
        {
            try
            {
                var relativePath = request.AttachmentPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(webHostEnvironment.WebRootPath, relativePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Ignore file deletion errors to ensure entity is deleted
            }
        }

        db.ServiceRequests.Remove(request);
        await db.SaveChangesAsync();
        return true;
    }

    private async Task<string> GenerateUniqueTrackingCodeAsync()
    {
        while (true)
        {
            var randomPart = Random.Shared.Next(10000, 99999);
            var code = $"TMZ-{randomPart}";
            var exists = await db.ServiceRequests.AnyAsync(r => r.TrackingCode == code);
            if (!exists)
            {
                return code;
            }
        }
    }
}

