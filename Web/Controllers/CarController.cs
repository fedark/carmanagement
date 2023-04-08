using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models;

namespace Web.Controllers;
public class CarController : Controller
{
    private static readonly string AllFilter = "All";

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

    public async Task<IActionResult> Index(CarSortViewModel? sortModel, CarFilterRequestModel? filter)
    {
        var cars = await context_.Cars.GetAllAsync();
        var viewCars = mapper_.Map<IEnumerable<CarViewModel>>(cars);

        var companies = viewCars.Select(c => c.Company).Distinct().OrderBy(c => c).ToList();
        companies.Insert(0, AllFilter);

        ApplySorting(ref viewCars, sortModel);
        ApplyFiltering(ref viewCars, filter);

        var filterModel = new CarFilterViewModel
        {
            Company = new SelectList(companies, filter?.Company ?? AllFilter),
            Model = filter?.Model,
            Year = filter?.Year,
            Displacement = filter?.Displacement
        };
        
        return View(new CarListViewModel { Cars = viewCars, SortModel = sortModel, FilterModel = filterModel });
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

        var car = mapper_.Map<Car>(viewCar);
        car.Picture = picture;
        await context_.Cars.AddAsync(car);

        logger_.LogInformation($"Added car with id '{car.Id}'");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var car = await context_.Cars.GetAsync(id);
        return View(mapper_.Map<CarEditModel>(car));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CarEditModel viewCar)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Status"] = "Some values are not provided.";
            return View(viewCar);
        }

        byte[] picture;
        string pictureType;

        if (viewCar.Picture is not null)
        {
            if (TryGetPictureBytes(viewCar.Picture, out var pictureBytes))
            {
                picture = pictureBytes;
                pictureType = viewCar.Picture.ContentType;
            }
            else
            {
                ViewData["Status"] = "The picture should be less than 4MB.";
                return View(viewCar);
            }
        }
        else
        {
            var oldCar = await context_.Cars.GetAsync(viewCar.Id);
            if (oldCar is not null)
            {
                picture = oldCar.Picture;
                pictureType = oldCar.PictureType;
            }
            else
            {
                ViewData["Status"] = "The picture should be provided.";
                return View(viewCar);
            }
        }
        
        var car = mapper_.Map<Car>(viewCar);
        car.Picture = picture;
        car.PictureType = pictureType;
        await context_.Cars.UpdateAsync(car);

        logger_.LogInformation($"Updated car with id '{car.Id}'");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var car = await context_.Cars.GetAsync(id);
        return View(mapper_.Map<CarViewModel>(car));
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        await context_.Cars.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Picture(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

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

    private void ApplySorting(ref IEnumerable<CarViewModel> cars, CarSortViewModel? sortModel)
    {
        if (sortModel is null)
        {
            return;
        }

        cars = sortModel switch
        {
            (CarSortKey.Company, true) => cars.OrderBy(c => c.Company),
            (CarSortKey.Company, false) => cars.OrderByDescending(c => c.Company),
            (CarSortKey.Model, true) => cars.OrderBy(c => c.Model),
            (CarSortKey.Model, false) => cars.OrderByDescending(c => c.Model),
            (CarSortKey.Year, true) => cars.OrderBy(c => c.Year),
            (CarSortKey.Year, false) => cars.OrderByDescending(c => c.Year),
            (CarSortKey.Displacement, true) => cars.OrderBy(c => c.Displacement),
            (CarSortKey.Displacement, false) => cars.OrderByDescending(c => c.Displacement),
            _ => throw new Exception("Invalid sort key")
        };
    }

    private void ApplyFiltering(ref IEnumerable<CarViewModel> cars, CarFilterRequestModel? filter)
    {
        if (filter is null)
        {
            return;
        }

        if (filter.Company is not null && filter.Company != AllFilter)
        {
            cars = cars.Where(c => c.Company.Contains(filter.Company, StringComparison.OrdinalIgnoreCase));
        }
        if (filter.Model is not null)
        {
            cars = cars.Where(c => c.Model.Contains(filter.Model, StringComparison.OrdinalIgnoreCase));
        }
        if (filter.Year is not null)
        {
            cars = cars.Where(c => c.Year == filter.Year);
        }
        if (filter.Displacement is not null)
        {
            cars = cars.Where(c => c.Displacement == filter.Displacement);
        }
    }
}
