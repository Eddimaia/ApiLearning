using AutoMapper;
using EstudoAPI.Data;
using EstudoAPI.Extensions;
using EstudoAPI.Models;
using EstudoAPI.Services;
using EstudoAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace EstudoAPI.Controllers;

[Route("api")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post([FromBody] RegisterViewModel model, IMapper mapper)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        try
        {
            var user = mapper.Map<User>(model);

            var password = PasswordGenerator.Generate(length: 25);

            user.PasswordHash = PasswordHasher.Hash(password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("MSG-E010.0 - Este E-mail já está cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("MSG-E02.0 - Falha interna no servidor"));
        }

    }
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email.Equals(model.Email));

            if (user is null)
                return StatusCode(401, new ResultViewModel<string>("MSG-E010.1 - Usuário ou Senha inválidos"));

            if(!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("MSG-E010.1 - Usuário ou Senha inválidos"));

            var token = tokenService.GenerateToken(user);

            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("MSG-E02.0 - Falha interna no servidor"));
        }


    }
}
