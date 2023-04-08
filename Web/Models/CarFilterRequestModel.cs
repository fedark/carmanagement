namespace Web.Models;

public class CarFilterRequestModel
{
    public string? Company { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public double? Displacement { get; set; }
}
