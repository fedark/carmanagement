using System.Security.Claims;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly IDataContext context_;

    public AuthController(IDataContext context)
    {
        context_ = context;
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

        var user = await context_.Users.GetByNameAsync(model.UserName);
        if (user is not null)
        {
            ViewData["Status"] = "The user name is already taken";
            return View(model);
        }

        user = new() { Name = model.UserName };

        var passwordHasher = new PasswordHasher<User>();
        var passwordHash = passwordHasher.HashPassword(user, model.Password);

        user.PasswordHash = passwordHash;

        await context_.Users.AddAsync(user);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Status"] = "Incorrect input";
            return View(model);
        }

        var user = await context_.Users.GetByNameAsync(model.UserName);
        if (user is null)
        {
            ViewData["Status"] = "The user is not registered";
            return View(model);
        }

        var passwordHasher = new PasswordHasher<User>();
        if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
        {
            ViewData["Status"] = "Incorrect input";
            return View(model);
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, user.Name) };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Denied()
    {
        return View();
    }
}
