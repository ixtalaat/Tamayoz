using Microsoft.EntityFrameworkCore;
using Tamayoz.Data;
using Tamayoz.Models;

namespace Tamayoz.Services;

public class TestimonialService(ApplicationDbContext db) : ITestimonialService
{
    public async Task<IReadOnlyList<Testimonial>> GetApprovedAsync(int count = 10)
    {
        return await db.Testimonials
            .Where(t => t.IsApproved)
            .OrderByDescending(t => t.ApprovedAt ?? t.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Testimonial>> GetAllForAdminAsync(bool? isApproved = null)
    {
        var query = db.Testimonials
            .OrderByDescending(t => t.CreatedAt)
            .AsQueryable();

        if (isApproved.HasValue)
        {
            query = query.Where(t => t.IsApproved == isApproved.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> SubmitAsync(Testimonial review)
    {
        review.IsApproved = false; // Must be approved by admin
        review.CreatedAt = DateTime.UtcNow;
        review.ApprovedAt = null;

        db.Testimonials.Add(review);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var item = await db.Testimonials.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        item.IsApproved = true;
        item.ApprovedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await db.Testimonials.FindAsync(id);
        if (item is null)
        {
            return false;
        }

        db.Testimonials.Remove(item);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await db.Testimonials.CountAsync(t => !t.IsApproved);
    }
}
