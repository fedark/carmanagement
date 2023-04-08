using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models;

public class CarFilterViewModel : ICloneable
{
    public SelectList Company { get; set; } = default!;
    public string? Model { get; set; }
    public int? Year { get; set; }
    public double? Displacement { get; set; }

    public object Clone()
    {
        return new CarFilterViewModel { Company = Company, Model = Model, Year = Year, Displacement = Displacement };
    }
}
