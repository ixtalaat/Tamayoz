using Tamayoz.Models;

namespace Tamayoz.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalServices { get; init; }
    public int ActiveServices { get; init; }
    public int PendingRequests { get; init; }
    public int InProgressRequests { get; init; }
    public int CompletedRequests { get; init; }
    public int UnreadMessages { get; init; }
    public IReadOnlyList<ServiceRequest> RecentRequests { get; init; } = [];
    public IReadOnlyList<ContactMessage> RecentMessages { get; init; } = [];
}
