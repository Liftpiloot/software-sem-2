using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IronApp.Models;
using System.Data.SqlClient;
using Iron_Domain;
using IronDomain;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ExerciseContainer _exerciseContainer = new ExerciseContainer();
    private ExerciseExecutionContainer _exerciseExecutionContainer = new ExerciseExecutionContainer();
    private UserContainer _userContainer = new UserContainer();
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

        var userId = Convert.ToInt32(Request.Cookies["UserId"]);
        _user.Id = userId;
        Console.WriteLine(_user.Id);
        _user.UserName = Request.Cookies["Username"] ?? string.Empty;
        _user.PasswordHash = Request.Cookies["PasswordHash"] ?? string.Empty;
        _user.DateOfBirth = Request.Cookies["DateOfBirth"] ?? string.Empty;
        _user.Weight = Convert.ToDecimal(Request.Cookies["Weight"]);

        // get selected exercises, and add sets if there is an execution. Create a List of ExerciseModel to pass to the view

        var selectedExercises = _exerciseContainer.GetSelectedExercises(_user);
        var exerciseModels = new List<ExerciseModel>();
        foreach (var selectedExercise in selectedExercises)
        {
            var exercise = _exerciseContainer.GetExerciseFromId(selectedExercise.ExerciseId);
            if (exercise == null) continue;
            var exerciseModel = new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            };

            var recentExecution = _exerciseExecutionContainer.GetRecentExerciseExecution(_user, exercise);
            if (recentExecution != null)
            {
                var sets = _exerciseExecutionContainer.GetSets(recentExecution);
                foreach (var set in sets)
                {
                    exerciseModel.Sets ??= [];

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

    [HttpGet]
    public IActionResult ExerciseList()
    {
        int userId = Convert.ToInt32(Request.Cookies["UserId"]);
        _user.Id = userId;
        List<ExerciseModel> exerciseModels = new List<ExerciseModel>();
        List<Exercise> exercises = _exerciseContainer.GetUnselectedExercises(_user);
        foreach (Exercise exercise in exercises)
        {
            ExerciseModel exerciseModel = new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            };
            exerciseModels.Add(exerciseModel);
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
        _exerciseContainer.AddSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult? Exercise(int ID)
    {
        Exercise? exercise = new Exercise
        {
            Id = ID
        };
        Console.WriteLine(exercise.Id);
        exercise = _exerciseContainer.GetExerciseFromId(exercise.Id);
        if (exercise != null)
        {
            ExerciseModel exerciseModel = new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            };
            return View(exerciseModel);
        }

        return null;
    }

    public IActionResult DeleteExercise(int id)
    {
        SelectedExercise selectedExercise = new SelectedExercise
        {
            UserId = Convert.ToInt32(Request.Cookies["UserId"]),
            ExerciseId = id
        };
        _exerciseContainer.DeleteSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult AddCustomExercise(string name)
    {
        var exercise = new Exercise
        {
            UserId = Convert.ToInt32(Request.Cookies["UserId"]),
            Name = name,
            Description = name,
            Logo = "/images/default.png"
        };
        var exerciseId = _exerciseContainer.AddExercise(exercise);
        if (exerciseId <= 0) return ExerciseList();
        SelectedExercise selectedExercise = new SelectedExercise
        {
            UserId = Convert.ToInt32(Request.Cookies["UserId"]),
            ExerciseId = exerciseId
        };
        _exerciseContainer.AddSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Home");
    }
}