using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public class RequestManagementService(ApplicationDbContext db) : IRequestManagementService
{
    public async Task<bool> CreateAsync(ServiceRequestViewModel model)
    {
        if (!await db.Services.AnyAsync(s => s.Id == model.ServiceId && s.IsActive)) return false;
        db.ServiceRequests.Add(new ServiceRequest { ServiceId = model.ServiceId, StudentName = model.StudentName, StudentEmail = model.StudentEmail, StudentPhone = model.StudentPhone, Message = model.Message, PreferredContactMethod = model.PreferredContactMethod });
        await db.SaveChangesAsync(); return true;
    }
    public async Task<IReadOnlyList<ServiceRequest>> GetAllAsync(RequestStatus? status)
    {
        var query = db.ServiceRequests.Include(r => r.Service).OrderByDescending(r => r.CreatedAt).AsQueryable();
        return await (status.HasValue ? query.Where(r => r.Status == status) : query).ToListAsync();
    }
    public async Task<bool> UpdateStatusAsync(int id, RequestStatus status)
    {
        var request = await db.ServiceRequests.FindAsync(id);
        if (request is null) return false;
        request.Status = status; request.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(); return true;
    }
}
