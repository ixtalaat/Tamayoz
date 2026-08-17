using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public class AdminDashboardService(ApplicationDbContext db) : IAdminDashboardService
{
    public async Task<AdminDashboardViewModel> GetAsync()
    {
        return new AdminDashboardViewModel
        {
            TotalServices = await db.Services.CountAsync(),
            ActiveServices = await db.Services.CountAsync(s => s.IsActive),
            PendingRequests = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.Pending),
            InProgressRequests = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.InProgress),
            CompletedRequests = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.Completed),
            UnreadMessages = await db.ContactMessages.CountAsync(m => !m.IsRead),
            RecentRequests = await db.ServiceRequests
                .Include(r => r.Service)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToListAsync(),
            RecentMessages = await db.ContactMessages
                .OrderBy(m => m.IsRead)
                .ThenByDescending(m => m.CreatedAt)
                .Take(5)
                .ToListAsync()
        };
    }
}

