using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class UserLoginModel
{
    [Display(Name = "User Name")]
    public string UserName { get; set; } = default!;

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}
