using AutoMapper;
using DapperAccess.Impl;
using Data.Access.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Authorize(Roles = RoleDefaults.Owner)]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IDataContext context_;
    private readonly IMapper mapper_;

    public UserController(IDataContext context, IMapper mapper)
    {
        context_ = context;
        mapper_ = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<UserResponceModel>> Get()
    {
        return mapper_.Map<IEnumerable<UserResponceModel>>(await context_.Users.GetAllAsync());
    }
}
