using AutoMapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController : ControllerBase
{
    private readonly IDataContext context_;
    private readonly IMapper mapper_;

    public CarController(IDataContext context, IMapper mapper)
    {
        context_ = context;
        mapper_ = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<CarResponceModel>> Get()
    {
        return (await context_.Cars.GetAllAsync()).Select(c => new CarResponceModel(c));
    }

    [HttpGet("{from:min(1)}/{to:min(1)}")]
    public async Task<IActionResult> Get(int from, int to)
    {
        if (from > to)
        {
            return BadRequest($"The '{nameof(from)}' parameter must be less or equal to the '{nameof(to)}' parameter.");
        }

        return new JsonResult((await context_.Cars.GetRangeAsync(from, to))
            .Select(c => new CarResponceModel(c)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var car = await context_.Cars.GetAsync(id);
        if (car is null)
        {
            return NotFound($"The object with ID '{id}' was not found.");
        }

        return new JsonResult(new CarResponceModel(car));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CarRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var car = mapper_.Map<Car>(model);
        await context_.Cars.AddAsync(car);

        return CreatedAtAction(nameof(Get), new { car.Id }, new CarResponceModel(car));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] CarRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var car = await context_.Cars.GetAsync(id);
        if (car is null)
        {
            return NotFound($"The object with ID '{id}' was not found.");
        }

        car = mapper_.Map<Car>(model);
        car.Id = id;

        await context_.Cars.UpdateAsync(car);

        return Ok();
    }

    [Authorize(Roles = "owner")]
    [HttpDelete("{id}")]
    public Task Delete(string id)
    {
        return context_.Cars.DeleteAsync(id);
    }
}
