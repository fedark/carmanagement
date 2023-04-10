using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly IDataContext contex_;

    public AuthController(IDataContext contex)
    {
        contex_ = contex;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Status"] = "Incorrect input";
            return View(model);
        }

        var user = await contex_.GetUserByNameAsync(model.UserName);
        if (user is not null)
        {
            ViewData["Status"] = "The user name is already taken";
            return View(model);
        }

        user = new() { Name = model.UserName };

        var passwordHasher = new PasswordHasher<User>();
        var passwordHash = passwordHasher.HashPassword(user, model.Password);

        user.PasswordHash = passwordHash;

        await contex_.Users.AddAsync(user);

        return RedirectToAction("Index", "Home");
    }
}
