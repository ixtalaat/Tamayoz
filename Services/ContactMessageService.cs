using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Services;

public class ContactMessageService(ApplicationDbContext db) : IContactMessageService
{
    public async Task CreateAsync(ContactMessage message) { db.ContactMessages.Add(message); await db.SaveChangesAsync(); }
    public async Task<IReadOnlyList<ContactMessage>> GetAllAsync() => await db.ContactMessages.OrderBy(m => m.IsRead).ThenByDescending(m => m.CreatedAt).ToListAsync();
    public async Task<bool> MarkReadAsync(int id)
    {
        var message = await db.ContactMessages.FindAsync(id);
        if (message is null) return false;
        message.IsRead = true; await db.SaveChangesAsync(); return true;
    }
    public async Task DeleteAsync(int id)
    {
        var message = await db.ContactMessages.FindAsync(id);
        if (message is null) return;
        db.ContactMessages.Remove(message); await db.SaveChangesAsync();
    }
}
