using Tamayoz.Data;
using Tamayoz.Models;
namespace Tamayoz.Services;
public class ContactMessageService(ApplicationDbContext db) : IContactMessageService { public async Task CreateAsync(ContactMessage message) { db.ContactMessages.Add(message); await db.SaveChangesAsync(); } }
