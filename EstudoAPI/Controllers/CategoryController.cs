using AutoMapper;
using EstudoAPI.Data;
using EstudoAPI.Extensions;
using EstudoAPI.Models;
using EstudoAPI.ViewModels;
using EstudoAPI.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EstudoAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public CategoryController(AppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var categories = _cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories();
                });

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("MSG-E02.0 - Falha interna no servidor"));
            }
        }

        private List<Category> GetCategories()
            => _context.Categories.AsNoTracking().ToList();

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(cat => cat.Id.Equals(id));

                if(category is null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E02.1 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = _mapper.Map<Category>(model);

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E01.2 - Não foi possível incluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E02.2 - Falha interna no servidor"));
            }

        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(cat => cat.Id.Equals(id));

                if (category is null)
                    return NotFound();

                _mapper.Map(model, category);

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Atualizado com sucesso." });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E01.3 - Não foi possível atualizar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E02.3 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(cat => cat.Id.Equals(id));

                if (category is null)
                    return NotFound();

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Deletado com sucesso." });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E01.4 - Não foi possível deletar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("MSG-E02.4 - Falha interna no servidor"));
            }
        }
    }
}
