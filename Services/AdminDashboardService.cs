using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Services;
public class AdminDashboardService(ApplicationDbContext db) : IAdminDashboardService
{
    public async Task<IReadOnlyDictionary<string, int>> GetStatisticsAsync() => new Dictionary<string, int>
    {
        ["إجمالي الخدمات"] = await db.Services.CountAsync(), ["الخدمات النشطة"] = await db.Services.CountAsync(s => s.IsActive),
        ["طلبات معلقة"] = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.Pending), ["طلبات قيد التنفيذ"] = await db.ServiceRequests.CountAsync(r => r.Status == RequestStatus.InProgress),
        ["رسائل جديدة"] = await db.ContactMessages.CountAsync(m => !m.IsRead)
    };
}
