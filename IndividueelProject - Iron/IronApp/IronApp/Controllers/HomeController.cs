using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IronApp.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using IronApp.Classes;

namespace IronApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private string db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    private User user = new User();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (Request.Cookies["userId"] == null)
        {
            return RedirectToAction("Index", "Login");
        }
        if (Request.Cookies["UserId"] != null)
        {
            user.Id = Convert.ToInt32(Request.Cookies["UserId"]);
            user.UserName = Request.Cookies["Username"];
            user.PasswordHash = Request.Cookies["PasswordHash"];
            user.DateOfBirth = Request.Cookies["DateOfBirth"];
            user.Weight = Convert.ToDecimal(Request.Cookies["Weight"]);
        }

        List<Exercise> exercises = user.GetRecentExercises();
        List<ExerciseModel> exercisemodels = new List<ExerciseModel>();
        foreach (Exercise exercise in exercises)
        {
            var info = exercise.GetExerciseInfo();
            List<Set> sets = exercise.GetSets();
            if (info is CustomExercise)
            {
                CustomExercise customExercise = (CustomExercise)info;
                exercisemodels.Add(new ExerciseModel
                {
                    Name = customExercise.Name, 
                    Description = customExercise.Description,
                    Sets = sets, 
                    Type = ExerciseType.Custom,
                    ExerciseTypeId = customExercise.Id
                });
                
            }
            else
            {
                PreDefinedExercise preDefinedExercise = (PreDefinedExercise)info;
                exercisemodels.Add(new ExerciseModel
                {
                    Name = preDefinedExercise.Name, 
                    Description = preDefinedExercise.Description, 
                    Logo = preDefinedExercise.Logo,
                    Sets = sets,
                    Type = ExerciseType.Predefined,
                    ExerciseTypeId = preDefinedExercise.Id
                });
            }
        }
        return View(exercisemodels);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult ExerciseList()
    {
        List<ExerciseModel> exercises = new List<ExerciseModel>();
        List<PreDefinedExercise> preDefinedExercises = user.GetPreDefinedExercises();
        List<CustomExercise> customExercises = user.GetCustomExercises();
        foreach (PreDefinedExercise exercise in preDefinedExercises)
        {
            exercises.Add(new ExerciseModel { Name = exercise.Name, Description = exercise.Description, Logo = exercise.Logo });
        }
        foreach (CustomExercise exercise in customExercises)
        {
            exercises.Add(new ExerciseModel { Name = exercise.Name, Description = exercise.Description});
        }
        
        return View(exercises);
    }

    public List<ExerciseModel> GetExercises()
    {
        Exercise exercise = new Exercise();
        int userid = Convert.ToInt32(Request.Cookies["UserId"]);
 
        exercise.GetExercises(userid);

        List<ExerciseModel> exercises = new List<ExerciseModel>();
        // fake list of exercises
        exercises.Add(new ExerciseModel { Name = "Bench Press", Description = "Lay on a bench and press the bar", Logo= "images/bench_wireframe.png" });
        exercises.Add(new ExerciseModel { Name = "Bicep Curl", Description = "Hold a dumbell, with one hand and lift it up engaging your bicep", Logo = "images/bicep_curl_wireframe.png" });
        exercises.Add(new ExerciseModel { Name = "Lat Pulldown", Description = "", Logo = "images/lat_pulldown_wireframe.png" });
        return exercises;
    }
}