using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Repositories.Interace
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryAsync(Guid? id);

        Task<Category> CreateCategoryAsync(Category category);

        Task<Category?> UpdateCategoryAsync(Category category);

        Task<IActionResult> DeleteCategoryAsync(Guid? id);
    }
}
