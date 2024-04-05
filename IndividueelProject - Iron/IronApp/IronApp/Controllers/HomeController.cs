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

        int userId = Convert.ToInt32(Request.Cookies["UserId"]);
        _user.Id = userId;
        Console.WriteLine(_user.Id);
        _user.UserName = Request.Cookies["Username"];
        _user.PasswordHash = Request.Cookies["PasswordHash"];
        _user.DateOfBirth = Request.Cookies["DateOfBirth"];
        _user.Weight = Convert.ToDecimal(Request.Cookies["Weight"]);

        // get selected exercises, and add sets if there is an execution. Create a List of ExerciseModel to pass to the view

        List<SelectedExercise> selectedExercises = _exerciseContainer.GetSelectedExercises(_user);
        List<ExerciseModel> exerciseModels = new List<ExerciseModel>();
        foreach (SelectedExercise selectedExercise in selectedExercises)
        {
            Exercise? exercise = _exerciseContainer.GetExerciseFromId(selectedExercise.ExerciseId);
            ExerciseModel exerciseModel = new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            };

            ExerciseExecution? recentExecution = _exerciseExecutionContainer.GetRecentExerciseExecution(_user, exercise);
            if (recentExecution != null)
            {
                int? executionId = recentExecution.Id;
                List<Set> sets = _exerciseExecutionContainer.GetSets(recentExecution);
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
        Exercise exercise = new Exercise
        {
            UserId = Convert.ToInt32(Request.Cookies["UserId"]),
            Name = name,
            Description = name,
            Logo = "/images/default.png"
            
        };
        int id = _exerciseContainer.AddExercise(exercise);
        SelectedExercise selectedExercise = new SelectedExercise
        {
            UserId = Convert.ToInt32(Request.Cookies["UserId"]),
            ExerciseId = id
        };
        _exerciseContainer.AddSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Home");
    }
}