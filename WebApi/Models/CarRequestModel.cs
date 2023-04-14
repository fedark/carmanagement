namespace WebApi.Models;

public class CarRequestModel
{
    public string Company { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int Year { get; set; }
    public double? Displacement { get; set; }
}
