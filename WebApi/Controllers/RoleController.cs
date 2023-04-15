using DapperAccess.Impl;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Authorize(Roles = RoleDefaults.Owner)]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IDataContext context_;

    public RoleController(IDataContext context)
    {
        context_ = context;
    }

    [HttpGet]
    public async Task<IEnumerable<string>> Get()
    {
        return (await context_.Roles.GetAllAsync()).Select(r => r.Name);
    }

    [HttpPost("{name}")]
    public async Task<IActionResult> Post(string name)
    {
        var role = await context_.Roles.GetByNameAsync(name);
        if (role is not null)
        {
            return BadRequest($"Role '{name}' already exists");
        }

        role = new Role { Name = name };
        await context_.Roles.AddAsync(role);

        return Ok();
    }

    [HttpPut("addtorole")]
    public async Task<IActionResult> AddToRole([FromBody] UserRoleModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await context_.Users.GetAsync(model.UserId);
        if (user is null)
        {
            return BadRequest($"The user with ID '{model.UserId}' was not found");
        }

        var role = await context_.Roles.GetByNameAsync(model.Role);
        if (role is null)
        {
            return BadRequest($"Role '{model.Role}' doesn't exist");
        }

        if (!user.Roles.Any(r => r.Id == role.Id))
        {
            user.Roles.Add(role);
            await context_.Users.UpdateAsync(user);
        }
        
        return Ok();
    }
}
