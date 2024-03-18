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
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    private User _user = new User();

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

        int userId = Convert.ToInt32(Request.Cookies["UserId"]);
        _user.Id = userId;
        Console.WriteLine(_user.Id);
        _user.UserName = Request.Cookies["Username"];
        _user.PasswordHash = Request.Cookies["PasswordHash"];
        _user.DateOfBirth = Request.Cookies["DateOfBirth"];
        _user.Weight = Convert.ToDecimal(Request.Cookies["Weight"]);

        // get selected exercises, and add sets if there is an execution. Create a List of ExerciseModel to pass to the view
        List<SelectedExercise> selectedExercises = _user.GetSelectedExercises();
        List<ExerciseModel> exerciseModels = new List<ExerciseModel>();
        foreach (SelectedExercise selectedExercise in selectedExercises)
        {
            Exercise exercise = selectedExercise.GetExercise();
            ExerciseModel exerciseModel = new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            };

            ExerciseExecution recentExecution = exercise.GetRecentExecution(userId);
            if (recentExecution != null)
            {
                int? executionId = recentExecution.Id;
                List<Set> sets = recentExecution.GetSets();
                foreach (Set set in sets)
                {
                    if (exerciseModel.Sets == null)
                    {
                        exerciseModel.Sets = new List<Set>();
                    }

                    exerciseModel.Sets.Add(set);
                }
            }

            exerciseModels.Add(exerciseModel);
        }

        return View(exerciseModels);
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
        int userId = Convert.ToInt32(Request.Cookies["UserId"]);
        _user.Id = userId;
        List<ExerciseModel> exerciseModels = new List<ExerciseModel>();
        List<Exercise> exercises = _user.GetExercises();
        List<SelectedExercise> selectedEx = _user.GetSelectedExercises();
        foreach (Exercise exercise in exercises)
        {
            bool selected = selectedEx.Any(x => x.ExerciseId == exercise.Id);
            if (selected)
            {
                continue;
            }

            exerciseModels.Add(new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            });
        }

        return View(exerciseModels);
    }

    [HttpPost]
    public IActionResult AddSelectedExercise(int id)
    {
        SelectedExercise selectedExercise = new SelectedExercise
        {
            UserId = Request.Cookies["UserId"] != null ? Convert.ToInt32(Request.Cookies["UserId"]) : 0,
            ExerciseId = id
        };
        selectedExercise.AddSelectedExercise();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Exercise(int ID)
    {
        Exercise exercise = new Exercise
        {
            Id = ID
        };
        Console.WriteLine(exercise.Id);
        exercise = exercise.GetExercise();
        Console.WriteLine(exercise.Id);
        ExerciseModel exerciseModel = new ExerciseModel
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            Logo = exercise.Logo
        };
        return View(exerciseModel);
    }

    public IActionResult DeleteExercise(int id)
    {
        SelectedExercise selectedExercise = new SelectedExercise
        {
            UserId = Convert.ToInt32(Request.Cookies["UserId"]),
            ExerciseId = id
        };
        selectedExercise.DeleteSelectedExercise();
        return RedirectToAction("Index", "Home");
    }
}