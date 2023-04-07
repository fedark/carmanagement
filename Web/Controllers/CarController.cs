using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;
public class CarController : Controller
{
    private readonly ILogger<CarController> logger_;
    private readonly IDataContext context_;
    private readonly IMapper mapper_;

    public CarController(ILogger<CarController> logger,
        IDataContext context,
        IMapper mapper)
    {
        logger_ = logger;
        context_ = context;
        mapper_ = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await context_.Cars.GetAllAsync();
        return View(mapper_.Map<IEnumerable<CarViewModel>>(cars));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CarCreateModel viewCar)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Status"] = "Some values are not provided.";
            return View(viewCar);
        }

        if (!TryGetPictureBytes(viewCar.Picture, out var picture))
        {
            ViewData["Status"] = "The picture should be less than 4MB.";
            return View(viewCar);
        }

        var company = new Company { Name = viewCar.Company };
        var model = new Model { Name = viewCar.Model, Year = viewCar.Year, Company = company };
        var car = new Car { Displacement = viewCar.Displacement, Picture = picture, PictureType = viewCar.Picture.ContentType, Model = model };
        await context_.Cars.AddAsync(car);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Picture(string id)
    {
        var car = await context_.Cars.GetAsync(id);
        if (car is null)
        {
            return NotFound($"Could not find car with id '{id}'");
        }

        return File(car.Picture, car.PictureType);
    }

    private bool TryGetPictureBytes(IFormFile pictureFile, [NotNullWhen(true)] out byte[]? picture)
    {
        if (pictureFile.Length == 0 || pictureFile.Length > 4 * 1024 * 1024)
        {
            picture = null;
            return false;
        }

        using var memoryStream = new MemoryStream();
        pictureFile.CopyTo(memoryStream);
        picture = memoryStream.ToArray();

        return true;
    }
}
