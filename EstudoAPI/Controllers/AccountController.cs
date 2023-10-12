using AutoMapper;
using EstudoAPI.Data;
using EstudoAPI.Extensions;
using EstudoAPI.Models;
using EstudoAPI.Services;
using EstudoAPI.ViewModels;
using EstudoAPI.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

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
    public async Task<IActionResult> Post([FromBody] RegisterViewModel model, IMapper mapper, EmailService emailService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var user = mapper.Map<User>(model);

            var password = PasswordGenerator.Generate(length: 25);

            user.PasswordHash = PasswordHasher.Hash(password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            emailService.Send(user.Name, user.Email, "Bem vindo a Estudo API!", $"Sua senha é <strong>{password}</<strong>");

            transaction.Commit();
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));
        }
        catch (DbUpdateException)
        {
            transaction.Rollback();
            return StatusCode(400, new ResultViewModel<string>("MSG-E010.0 - Este E-mail já está cadastrado"));
        }
        catch(Exception ex)
        {
            transaction.Rollback();
            await _context.SaveChangesAsync(false);
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

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("MSG-E010.1 - Usuário ou Senha inválidos"));

            var token = tokenService.GenerateToken(user);

            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("MSG-E02.0 - Falha interna no servidor"));
        }
    }
    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage([FromBody] UploadImageViewModel model)
    {
        var fileName = $"{Guid.NewGuid()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("MSG-E02.0 - Falha interna no servidor"));
        }

        var user = await _context
            .Users
            .FirstOrDefaultAsync(x => x.Email.Equals(User.Identity.Name));

        if (user == null)
            return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

        user.Image = $"https://localhost:7290/images/{fileName}";
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }

        return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!", null));
    }
}
