using Tamayoz.Models;
using Tamayoz.ViewModels;

namespace Tamayoz.Services;

public interface IRequestManagementService
{
    Task<bool> CreateAsync(ServiceRequestViewModel model);
    Task<IReadOnlyList<ServiceRequest>> GetAllAsync(RequestStatus? status);
    Task<bool> UpdateStatusAsync(int id, RequestStatus status);
}
