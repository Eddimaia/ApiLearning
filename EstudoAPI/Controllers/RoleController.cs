using AutoMapper;
using EstudoAPI.Data;
using EstudoAPI.Extensions;
using EstudoAPI.Models;
using EstudoAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EstudoAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RoleController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<RoleController>
        [HttpGet("v1/roles")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var roles = await _context.Roles
                .AsNoTracking()
                .ToListAsync();

                return Ok(new ResultViewModel<List<Role>>(roles));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Role>>("MSG-E02.0 - Falha interna no servidor"));
            }
        }

        // GET api/<RoleController>/5
        [HttpGet("v1/roles/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(rol => rol.Id.Equals(id));

                if (role is null)
                    return NotFound(new ResultViewModel<Role>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Role>(role));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("MSG-E02.1 - Falha interna no servidor"));
            }
        }

        // POST api/<RoleController>
        [HttpPost("v1/roles")]
        public async Task<IActionResult> PostAsync([FromBody] EditorRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));

            try
            {
                var role = _mapper.Map<Role>(model);

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return Created($"v1/roles/{role.Id}", new ResultViewModel<Role>(role));
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

        // PUT api/<RoleController>/5
        [HttpPut("v1/roles/{id:int}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] EditorRoleViewModel model)
        {
            try
            {
                var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(rol => rol.Id.Equals(id));

                if (role is null)
                    return NotFound();

                _mapper.Map(model, role);

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Atualizado com sucesso." });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("MSG-E01.3 - Não foi possível atualizar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("MSG-E02.3 - Falha interna no servidor"));
            }
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("v1/roles/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(rol => rol.Id.Equals(id));

                if (role is null)
                    return NotFound();

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Deletado com sucesso." });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Role>("MSG-E01.4 - Não foi possível deletar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Role>("MSG-E02.4 - Falha interna no servidor"));
            }
        }
    }
}
