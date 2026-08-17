using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public class RequestManagementService(ApplicationDbContext db) : IRequestManagementService
{
    public async Task<bool> CreateAsync(ServiceRequestViewModel model)
    {
        var isServiceActive = await db.Services.AnyAsync(s => s.Id == model.ServiceId && s.IsActive);
        if (!isServiceActive)
        {
            return false;
        }

        var entity = new ServiceRequest
        {
            ServiceId = model.ServiceId,
            StudentName = model.StudentName,
            StudentEmail = model.StudentEmail,
            StudentPhone = model.StudentPhone,
            Message = model.Message,
            PreferredContactMethod = model.PreferredContactMethod,
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

