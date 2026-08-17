using Tamayoz.Models;

namespace Tamayoz.Services;

public interface IContactMessageService
{
    Task CreateAsync(ContactMessage message);
    Task<IReadOnlyList<ContactMessage>> GetAllAsync();
    Task<bool> MarkReadAsync(int id);
    Task DeleteAsync(int id);
}

