using System.Security.Cryptography;
using System.Text;
using IronApp.Models;
using IronDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class RegisterController : Controller
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult Index(RegisterModel model)
    {
        Console.WriteLine(model.DateOfBirth);
        if (ModelState.IsValid)
        {
            
            
            User? user = new User(model.Username, model.Email, model.Password, model.DateOfBirth, model.Weight);
            string response = user.AddUser();
            if (response != "Success")
            {
                ModelState.AddModelError("Username", response);
                return View(model);
            }
            
            // Login user
            user = user.Login();
            
            // save user in cookies
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
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and confirmation password do not match.");
            }

            if (model.Password.Length < 10)
            {
                ModelState.AddModelError("Password", "The password must be at least 10 characters long.");
            }

            if (model.DateOfBirth == null)
            {
                ModelState.AddModelError("DateOfBirth", "Date of birth is not required.");
            }
            else
            {
                DateTime dob;
                if (!DateTime.TryParse(model.DateOfBirth.ToString(), out dob))
                {
                    ModelState.AddModelError("DateOfBirth", "Invalid date format.");
                }
            }

            if (model.Weight <= 0)
            {
                ModelState.AddModelError("Weight", "Weight is required.");
            }

            return View(model);
        }
    }
}