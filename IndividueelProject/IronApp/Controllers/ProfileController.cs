using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using Iron_DAL;
using Iron_Domain;
using Iron_Interface;
using IronApp.Models;
using IronDomain;
using Microsoft.AspNetCore.Mvc;

namespace IronApp.Controllers;

public class ProfileController : Controller
{
    private readonly IDbUser _dbUser;
    private readonly UserContainer _userContainer;
    private readonly IDbExerciseExecution _dbExerciseExecution;
    private readonly ExerciseExecutionContainer _exerciseExecutionContainer;

    public ProfileController()
    {
        _dbUser = new DbUser();
        _userContainer = new UserContainer(_dbUser);
        _dbExerciseExecution = new DbExerciseExecution();
        _exerciseExecutionContainer = new ExerciseExecutionContainer(_dbExerciseExecution);
    }

    public IActionResult Index()
    {
        ProfileModel profileModel = new ProfileModel();
        // Get user data from cookies
        profileModel.UserModel = new UserModel();
        bool loggedIn = int.TryParse(Request.Cookies["UserId"], out int userId);
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Login");
        }
        
        profileModel.UserModel.Id = userId;
        User? user = _userContainer.GetUser(profileModel.UserModel.Id);
        if (user == null)
        {
            return RedirectToAction("Index", "Login");
        }

        profileModel.UserModel.Username = user.UserName;
        profileModel.UserModel.Email = user.Email;
        profileModel.UserModel.DateOfBirth = user.DateOfBirth;
        profileModel.UserModel.Weight = user.Weight;

        // Get graph data
        profileModel.WorkoutsPerWeek = _exerciseExecutionContainer.GetWorkoutsPerWeek(profileModel.UserModel.Id)
            .Select(w => new Dictionary<string, int> { { "week", w.Item1 }, { "workouts", w.Item2 } }).ToList();

        // Get other data
        if (profileModel.WorkoutsPerWeek.Any())
        {
            profileModel.AveragePerWeek = Math.Round(profileModel.WorkoutsPerWeek.Average(x => x["workouts"]), 2);
        }
        else
        {
            profileModel.AveragePerWeek = 0; // Or any other default value
        }
        profileModel.TotalVolume = _exerciseExecutionContainer.GetTotalVolume(profileModel.UserModel.Id);
        return View(profileModel);
    }

    public IActionResult EditWeight(decimal weight)
    {
        if (weight <= 0)
        {
            return RedirectToAction("Index");
        }

        bool loggedIn = int.TryParse(Request.Cookies["UserId"], out int userId);
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Login");
        }
        _userContainer.EditWeight(userId, weight);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordModel model)
    {
        // Check if user is logged in
        bool loggedIn = int.TryParse(Request.Cookies["UserId"], out int userId);
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Login");
        }

        User? user = _userContainer.GetUser(userId);
        if (user == null)
        {
            return RedirectToAction("Index", "Login");
        }

        // Check if old password is correct
        user.PasswordHash = model.OldPassword;
        user.HashPassword();
        user = _userContainer.Login(user);

        if (user == null)
        {
            ModelState.AddModelError("OldPassword", "Old password is incorrect.");
            return View(model);
        }

        if (model.NewPassword != model.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "Password and confirmation password do not match.");
            return View(model);
        }

        if (model.NewPassword.Length < 10)
        {
            ModelState.AddModelError("NewPassword", "Password must be at least 10 characters long.");
            return View(model);
        }

        if (_userContainer.ChangePassword(user.Id, model.NewPassword))
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("NewPassword", "An error occurred.");
        return View(model);
    }
    
    public IActionResult Logout()
    {
        Response.Cookies.Delete("UserId");
        return RedirectToAction("Index", "Login");
    }
}