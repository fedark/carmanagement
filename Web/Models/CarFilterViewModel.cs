using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models;

public class CarFilterViewModel
{
    public SelectList Company { get; set; } = default!;
    public string? Model { get; set; }
    public int? Year { get; set; }
    public double? Displacement { get; set; }
}
