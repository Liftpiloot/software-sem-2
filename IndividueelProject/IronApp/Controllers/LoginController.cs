using System.Security.Cryptography;
using System.Text;
using Iron_DAL;
using Iron_Domain;
using Iron_Interface;
using IronApp.Models;
using IronDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class LoginController : Controller
{
    private readonly UserContainer _userContainer;
    private readonly IDbUser _dbUser;
    public LoginController()
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
    public IActionResult Index(LoginModel model)
    {
        var user = new User(0, model.Name, model.Name, model.Password, DateTime.Now, 0);
        // Hash password
        user.HashPassword();
        user = _userContainer.Login(user);
        if (user == null)
        {
            ModelState.Clear();
            ModelState.AddModelError("Username", "Login failed");
            return View(model);
        }
        
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(365), // Cookie expires after a year
            IsEssential = true,
            Secure = true,
            HttpOnly = true
        };
        Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
        Response.Cookies.Append("Username", user.UserName, cookieOptions);
        Response.Cookies.Append("PasswordHash", user.PasswordHash, cookieOptions);
        Response.Cookies.Append("Email", user.Email, cookieOptions);
        Response.Cookies.Append("DateOfBirth", user.DateOfBirth.ToString(), cookieOptions);
        Response.Cookies.Append("Weight", user.Weight.ToString(), cookieOptions);
        return RedirectToAction("Index", "Home");

    }
}