using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class CarEditModel
{
    public string Id { get; set; } = default!;

    [Display(Name = "Company")]
    public string Company { get; set; } = default!;

    [Display(Name = "Model")]
    public string Model { get; set; } = default!;

    [Display(Name = "Year Manufactured")]
    public int Year { get; set; }

    [Display(Name = "Displacement")]
    public double? Displacement { get; set; }

    [Display(Name = "Picture")]
    [DataType(DataType.Upload)]
    public IFormFile? Picture { get; set; }
}
