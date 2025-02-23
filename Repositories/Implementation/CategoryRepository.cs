using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interace;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodePulse.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(Guid? id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);

            await dbContext.SaveChangesAsync();

            //dbContext.Categories.

            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            var result = await dbContext.Categories.FirstOrDefaultAsync(e => e.Id == category.Id);
            if (result != null)
            {
                result.Name = category.Name;
                result.UrlHandle = category.UrlHandle;

                await dbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<IActionResult> DeleteCategoryAsync(Guid? id)
        {
            var result = await dbContext.Categories.FindAsync(id);
            //var result = await dbContext.Categories.ToListAsync();
            if (result != null)
            {
                dbContext.Categories.Remove(result);
                await dbContext.SaveChangesAsync();
                //return result;
            }
            return null;
        }
    }
}
