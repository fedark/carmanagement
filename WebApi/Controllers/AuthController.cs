using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IDataContext context_;
    private readonly JwtService jwtService_;

    public AuthController(IDataContext context, JwtService jwtService)
    {
        context_ = context;
        jwtService_ = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (await context_.Users.GetByNameAsync(model.Name) is not null)
        {
            return BadRequest($"The user with name '{model.Name}' is already registered.");
        }

        var user = new User { Name = model.Name };

        var passwordHasher = new PasswordHasher<User>();
        var passwordHash = passwordHasher.HashPassword(user, model.Password);
        user.PasswordHash = passwordHash;

        await context_.Users.AddAsync(user);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await context_.Users.GetByNameAsync(model.Name);
        if (user is null)
        {
            return Unauthorized($"The user with name '{model.Name}' is not registered");
        }

        var passwordHasher = new PasswordHasher<User>();
        if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
        {
            return Unauthorized($"Incorrect password for user '{user.Name}'");
        }

        var jwt = jwtService_.GetToken(user);
        return new JsonResult(new { UserName = user.Name, Jwt = jwt });
    }
}
