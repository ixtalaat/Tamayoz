using Tamayoz.Models;

namespace Tamayoz.Services;

public interface IServiceCatalogService
{
    Task<IReadOnlyList<Service>> GetActiveAsync(int? take = null);
    Task<IReadOnlyList<Service>> GetAllAsync();
    Task<Service?> GetActiveByIdAsync(int id);
    Task<Service?> GetByIdAsync(int id);
    Task CreateAsync(Service service);
    Task<bool> UpdateAsync(Service service);
    Task RemoveOrDeactivateAsync(int id);
}
