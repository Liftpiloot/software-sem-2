﻿using System.Security.Cryptography;
using System.Text;
using Iron_Domain;
using IronApp.Models;
using IronDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class RegisterController : Controller
{
    UserContainer _userContainer = new UserContainer();

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
            var user = new User(model.Username, model.Email, model.Password, model.DateOfBirth, model.Weight); 
            var userId = _userContainer.AddUser(user);
            switch (userId)
            {
                case 0:
                    ModelState.AddModelError("Username", "Username or email already exists.");
                    return View(model);
                case -1:
                    ModelState.AddModelError("Username", "An error occurred.");
                    return View(model);
            }
            
            // Login user
            user = _userContainer.Login(user);
            
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