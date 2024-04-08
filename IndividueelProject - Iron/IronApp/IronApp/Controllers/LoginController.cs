using System.Security.Cryptography;
using System.Text;
using Iron_Domain;
using IronDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class LoginController : Controller
{
    private readonly UserContainer _userContainer = new();
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    // POST
    [HttpPost]
    public IActionResult Index(string name, string password)
    {
        var user = new User
        {
            UserName = name,
            Email = name
        };
        // Hash password
        using (SHA256 sha256Hash = SHA256.Create())
        {
            Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            password = builder.ToString();
        }
        user.PasswordHash = password;
        user = _userContainer.Login(user);
        if (user == null) return RedirectToAction("Index", "Login");
        
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(365), // Cookie expires after a year
            IsEssential = true
        };
        Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
        Response.Cookies.Append("Username", user.UserName, cookieOptions);
        Response.Cookies.Append("PasswordHash", user.PasswordHash, cookieOptions);
        Response.Cookies.Append("DateOfBirth", user.DateOfBirth.ToString(), cookieOptions);
        Response.Cookies.Append("Weight", user.Weight.ToString(), cookieOptions);
        return RedirectToAction("Index", "Home");

    }
}