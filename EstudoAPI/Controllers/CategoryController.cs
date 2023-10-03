using AutoMapper;
using EstudoAPI.Data;
using EstudoAPI.DTO;
using EstudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstudoAPI.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync()
        {
            var cartegories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return Ok(cartegories);
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(cat => cat.Id.Equals(id));

            return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "MSG-E02.1 - Falha interna no servidor");
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryDTO model)
        {
            try
            {
                var category = _mapper.Map<Category>(model);

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "MSG-E01.2 - Não foi possível incluir a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "MSG-E02.2 - Falha interna no servidor");
            }

        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryDTO model)
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
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "MSG-E01.3 - Não foi possível atualizar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "MSG-E02.3 - Falha interna no servidor");
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
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "MSG-E01.4 - Não foi possível deletar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "MSG-E02.4 - Falha interna no servidor");
            }
        }
    }
}
