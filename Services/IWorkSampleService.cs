using Microsoft.AspNetCore.Http;
using Tamayoz.Models;

namespace Tamayoz.Services;

public interface IWorkSampleService
{
    Task<IReadOnlyList<WorkSample>> GetActiveSamplesAsync(string? category = null);
    Task<IReadOnlyList<WorkSample>> GetAllForAdminAsync();
    Task<WorkSample?> GetByIdAsync(int id);
    Task<bool> CreateAsync(WorkSample sample, IFormFile? thumbnailFile, IFormFile? sampleFile);
    Task<bool> UpdateAsync(WorkSample sample, IFormFile? thumbnailFile, IFormFile? sampleFile);
    Task<bool> DeleteAsync(int id);
    Task<bool> ToggleActiveAsync(int id);
    Task<IReadOnlyList<string>> GetCategoriesAsync();
}
