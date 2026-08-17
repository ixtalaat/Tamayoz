using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Services;

public class ServiceCatalogService(ApplicationDbContext db) : IServiceCatalogService
{
    public async Task<IReadOnlyList<Service>> GetActiveAsync(int? take = null)
    {
        var query = db.Services.Where(s => s.IsActive).OrderByDescending(s => s.CreatedAt);
        return await (take.HasValue ? query.Take(take.Value) : query).ToListAsync();
    }

    public async Task<IReadOnlyList<Service>> GetAllAsync()
    {
        return await db.Services.OrderByDescending(s => s.CreatedAt).ToListAsync();
    }

    public Task<Service?> GetActiveByIdAsync(int id)
    {
        return db.Services.FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public Task<Service?> GetByIdAsync(int id)
    {
        return db.Services.FindAsync(id).AsTask();
    }

    public async Task CreateAsync(Service service)
    {
        service.CreatedAt = DateTime.UtcNow;
        db.Services.Add(service);
        await db.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Service model)
    {
        var service = await db.Services.FindAsync(model.Id);
        if (service is null)
        {
            return false;
        }

        service.Name = model.Name;
        service.ShortDescription = model.ShortDescription;
        service.Description = model.Description;
        service.Price = model.Price;
        service.EstimatedDuration = model.EstimatedDuration;
        service.ImageUrl = model.ImageUrl;
        service.IsActive = model.IsActive;
        service.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task RemoveOrDeactivateAsync(int id)
    {
        var service = await db.Services.Include(s => s.Requests).FirstOrDefaultAsync(s => s.Id == id);
        if (service is null)
        {
            return;
        }

        if (service.Requests.Count == 0)
        {
            db.Services.Remove(service);
        }
        else
        {
            service.IsActive = false;
            service.UpdatedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();
    }
}

