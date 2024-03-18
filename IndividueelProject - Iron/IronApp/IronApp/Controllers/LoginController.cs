using System.Security.Cryptography;
using System.Text;
using IronApp.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class LoginController : Controller
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    // GET
    public IActionResult Index()
    {
        return PartialView();
    }
    // POST
    [HttpPost]
    public IActionResult Index(string? name, string? password)
    {
        User? user = new User();
        user.UserName = name;
        // Hash password
        using (SHA256 sha256Hash = SHA256.Create())
        {
            Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            password = builder.ToString();
        }
        user.PasswordHash = password;
        user = user.Login();
        if (user != null){
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(365), // Cookie expires after a year
                IsEssential = true
            };
            Response.Cookies.Append("UserId", user.Id.ToString(), cookieOptions);
            Response.Cookies.Append("Username", user.UserName, cookieOptions);
            Response.Cookies.Append("PasswordHash", user.PasswordHash, cookieOptions);
            Response.Cookies.Append("DateOfBirth", user.DateOfBirth, cookieOptions);
            Response.Cookies.Append("Weight", user.Weight.ToString(), cookieOptions);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }
}