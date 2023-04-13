using Data.Models;

namespace WebApi.Models;

public record CarDto(string Id, string Company, string Model, int Year, double? Displacement)
{
    public CarDto(Car car) : this(car.Id, car.Model.Company.Name, car.Model.Name, car.Model.Year, car.Displacement)
    {
    }
}
