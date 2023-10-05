using EstudoAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstudoAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("v1/users")]
        public IActionResult Get()
        {
            var users = _context.Users.AsNoTracking().ToList();
            return Ok(users);
        }
    }
}
