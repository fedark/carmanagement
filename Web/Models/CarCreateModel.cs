using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class CarCreateModel
{
    [Display(Name = "Company")]
    public string Company { get; set; } = default!;

    [Display(Name = "Model")]
    public string Model { get; set; } = default!;

    [Display(Name = "Year Manufactured")]
    public int Year { get; set; }

    [Display(Name = "Displacement")]
    public int? Displacement { get; set; }

    [Display(Name = "Picture")]
    [DataType(DataType.Upload)]
    public IFormFile Picture { get; set; } = default!;
}
