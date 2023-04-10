using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class UserRegisterModel
{
    [Display(Name = "User Name")]
    public string UserName { get; set; } = default!;

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = default!;
}
