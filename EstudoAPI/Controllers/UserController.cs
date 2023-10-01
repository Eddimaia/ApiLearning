using EstudoAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstudoAPI.Controllers
{
    [ApiController]
    [Route("v1/{controller}")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("users")]
        public IActionResult Get()
        {
            var user = _context.Users.AsNoTracking().ToList();
            return Ok(user);
        }
    }
}
