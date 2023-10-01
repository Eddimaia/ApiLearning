using EstudoAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstudoAPI.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public IActionResult Get()
        {
            var cartegories = _context.Categories
                .AsNoTracking()
                .ToList();

            return Ok(cartegories);
        }
    }
}
