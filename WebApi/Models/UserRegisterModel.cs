using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class UserRegisterModel
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;

    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = default!;
}
