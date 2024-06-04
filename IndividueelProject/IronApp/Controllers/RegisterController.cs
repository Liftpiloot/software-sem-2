using Iron_DAL;
using Iron_Domain;
using Iron_Interface;
using IronApp.Models;
using IronDomain;
using Microsoft.AspNetCore.Mvc;

namespace IronApp.Controllers;

public class RegisterController : Controller
{
    private readonly UserContainer _userContainer;
    private readonly IDbUser _dbUser;

    public RegisterController()
    {
        _dbUser = new DbUser();
        _userContainer = new UserContainer(_dbUser);
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult Index(RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError("Password", "Passwords do not match.");
            return View(model);
        }
        var user = new User(0, model.Username, model.Email, model.Password, model.DateOfBirth, model.Weight);
        // Hash password
        user.HashPassword();
        var (userId, errors) = _userContainer.AddUser(user);
        switch (userId)
        {
            case 0:
                ModelState.AddModelError("Username", "Username or email already exists.");
                return View(model);
            case -1:
                if (errors.Count==0)
                {
                    ModelState.AddModelError("Username", "An error occurred.");
                    return View(model);
                }
                foreach (var error in errors)
                {
                    if (error.PropertyName == "PasswordHash")
                    {
                        ModelState.AddModelError("Password", error.ErrorMessage);
                    }
                    else
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }

                return View(model);
        }

        // Login user
        user = _userContainer.Login(user);
        if (user == null)
        {
            ModelState.AddModelError("Username", "An error occurred.");
            return View(model);
        }

        // save user in cookies
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(365), // Cookie expires after a year
            IsEssential = true,
            Secure = true,
            HttpOnly = true
        };
        Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);

        return RedirectToAction("Index", "Exercise");
    }
}