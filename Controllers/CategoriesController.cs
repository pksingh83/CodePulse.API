using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.Domain;
using CodePulse.API.Data;
using CodePulse.API.Repositories.Interace;

namespace CodePulse.API.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly ApplicationDbContext dbContext;
        private readonly ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)//ApplicationDbContext dbContext
        {
            //this.dbContext = dbContext;
            this.categoryRepository = categoryRepository;
            
        }

        [HttpGet("getCategories")]///api/Categories/GetCategories
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                
                return Ok(await categoryRepository.GetCategoriesAsync());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("getCategory/{id}")]///api/Categories/GetCategory
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            try
            {
                var result = await categoryRepository.GetCategoryAsync(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost("createCategory")]//"/api/Categories/CreateCategory"
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryRequestDto request)
        {
            if (request == null)
                return BadRequest();

            //Map Dto to Domain model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            await categoryRepository.CreateCategoryAsync(category);

            // Domain model to Dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            return Ok(response);
        }

        [HttpPut("updateCategory/{id}")]///api/Categories/UpdateCategory
        public async Task<ActionResult<Category>> UpdateCategory(Guid id, CreateCategoryRequestDto request)
        { 
            var category = await categoryRepository.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var updateCategory = new Category 
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.UpdateCategoryAsync(updateCategory);
            return RedirectToAction(nameof(GetCategories));
        }

        [HttpDelete("deleteCategory/{id}")]///api/Categories/DeleteCategory
        public async Task<ActionResult<Category>> DeleteCategory(Guid id)
        {
            var category = await categoryRepository.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await categoryRepository.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(GetCategories));
        }
    }
}
