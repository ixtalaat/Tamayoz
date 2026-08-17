using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public interface IRequestManagementService
{
    Task<string?> CreateAsync(ServiceRequestViewModel model);
    Task<ServiceRequest?> GetByTrackingCodeAsync(string trackingCode);
    Task<IReadOnlyList<ServiceRequest>> GetByPhoneAsync(string phone);
    Task<IReadOnlyList<ServiceRequest>> GetAllAsync(RequestStatus? status);
    Task<bool> UpdateStatusAsync(int id, RequestStatus status);
}
