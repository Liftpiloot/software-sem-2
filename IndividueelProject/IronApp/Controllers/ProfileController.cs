using System.Runtime.InteropServices.JavaScript;
using Iron_DAL;
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
        profileModel.UserModel.Id = int.Parse(Request.Cookies["UserId"] ?? string.Empty);
        profileModel.UserModel.Username = Request.Cookies["Username"] ?? string.Empty;
        profileModel.UserModel.Email = Request.Cookies["Email"] ?? string.Empty;
        profileModel.UserModel.Weight = decimal.Parse(Request.Cookies["Weight"] ?? string.Empty);
        profileModel.UserModel.DateOfBirth = DateTime.Parse(Request.Cookies["DateOfBirth"] ?? string.Empty);
        
        // Get graph data
        profileModel.WorkoutsPerWeek = _exerciseExecutionContainer.GetWorkoutsPerWeek(profileModel.UserModel.Id).Select(w => new Dictionary<string, int> {{"week", w.Item1}, {"workouts", w.Item2}}).ToList();
        
        // Get other data
        profileModel.AveragePerWeek = Math.Round(profileModel.WorkoutsPerWeek.Average(x => x["workouts"]), 2);
        profileModel.TotalVolume = 20;
        return View(profileModel);
    }

    public IActionResult EditWeight(decimal weight)
    {
            if (weight <= 0)
            {
                return RedirectToAction("Index");
            }
            int userId = int.Parse(Request.Cookies["UserId"] ?? string.Empty);
            if (_userContainer.EditWeight(userId, weight))
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(365), // Cookie expires after a year
                    IsEssential = true,
                    Secure = true,
                    HttpOnly = true
                };
                Response.Cookies.Append("Weight", weight.ToString(), cookieOptions);
            }
            return RedirectToAction("Index");
        
        return RedirectToAction("Index");
    }
}