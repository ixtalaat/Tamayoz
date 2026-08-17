namespace Tamayoz.Services;
public interface IAdminDashboardService { Task<IReadOnlyDictionary<string, int>> GetStatisticsAsync(); }
