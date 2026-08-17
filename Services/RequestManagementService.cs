using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public class RequestManagementService(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) : IRequestManagementService
{
    public async Task<bool> CreateAsync(ServiceRequestViewModel model)
    {
        var isServiceActive = await db.Services.AnyAsync(s => s.Id == model.ServiceId && s.IsActive);
        if (!isServiceActive)
        {
            return false;
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

        var entity = new ServiceRequest
        {
            ServiceId = model.ServiceId,
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
        return true;
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
}

