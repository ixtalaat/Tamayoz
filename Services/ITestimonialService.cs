using Tamayoz.Models;

namespace Tamayoz.Services;

public interface ITestimonialService
{
    Task<IReadOnlyList<Testimonial>> GetApprovedAsync(int count = 10);
    Task<IReadOnlyList<Testimonial>> GetAllForAdminAsync(bool? isApproved = null);
    Task<bool> SubmitAsync(Testimonial review);
    Task<bool> ApproveAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<int> GetPendingCountAsync();
}
