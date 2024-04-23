using System.Diagnostics;
using Iron_DAL;
using Microsoft.AspNetCore.Mvc;
using IronApp.Models;
using Iron_Domain;
using Iron_Interface;
using IronDomain;

namespace IronApp.Controllers;

public class HomeController : Controller
{
    private readonly ExerciseContainer _exerciseContainer;
    private readonly ExerciseExecutionContainer _exerciseExecutionContainer;
    private readonly IDbExercise _dbExercise;
    private readonly IDbExerciseExecution _dbExerciseExecution;
    private readonly User _user = new();
    
    public HomeController()
    {
        _dbExercise = new DbExercise();
        _dbExerciseExecution = new DbExerciseExecution();
        _exerciseContainer = new ExerciseContainer(_dbExercise);
        _exerciseExecutionContainer = new ExerciseExecutionContainer(_dbExerciseExecution);
        
    }
    
    public IActionResult Index()
    {
        if (Request.Cookies["userId"] == null)
        {
            return RedirectToAction("Index", "Login");
        }

        var userId = Convert.ToInt32(Request.Cookies["UserId"]);
        _user.Id = userId;
        _user.UserName = Request.Cookies["Username"] ?? string.Empty;
        _user.PasswordHash = Request.Cookies["PasswordHash"] ?? string.Empty;
        _user.DateOfBirth = Convert.ToDateTime(Request.Cookies["DateOfBirth"]);
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

    public IActionResult? Exercise(int id)
    {
        Exercise? exercise = new Exercise
        {
            Id = id
        };
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
            _user.Id = Convert.ToInt32(Request.Cookies["UserId"]);
            List<List<SetModel>> allSets = new List<List<SetModel>>();
            List<ExerciseExecution> exerciseExecutions = _exerciseExecutionContainer.GetExerciseExecutions(_user, exercise);
            foreach (ExerciseExecution exerciseExecution in exerciseExecutions)
            {
                List<Set> executionSets = _exerciseExecutionContainer.GetSets(exerciseExecution);
                if (executionSets.Count == 0) continue;
                List<SetModel> executionSetsModel = new List<SetModel>();
                foreach (Set set in executionSets)
                {
                    SetModel setModel = new SetModel
                    {
                        Reps = set.Reps,
                        Weight = set.Weight,
                        Date = exerciseExecution.ExecutionDate
                    };
                    executionSetsModel.Add(setModel);
                }
                allSets.Add(executionSetsModel);
            }
            List<DataPointModel> dataPoints = new List<DataPointModel>();
            foreach (List<SetModel> sets in allSets)
            {
                // Get highest weight
                decimal highestWeight = sets.Select(set => set.Weight).Prepend(0).Max();
                // Get date
                DateTime date = sets[0].Date;
                DataPointModel dataPoint = new DataPointModel(date, highestWeight);
                dataPoints.Add(dataPoint);
            }
            exerciseModel.DataPoints = dataPoints;
            
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

    [HttpPost]
    public IActionResult SaveWorkout([FromBody] List<WorkoutEntryModel> workout)
    {
        if (workout.Count == 0)
        {
            return BadRequest("No exercises in workout");
        }
        decimal volume = 0;
        List<ExerciseExecution> exerciseExecutions = new List<ExerciseExecution>();
        foreach (var exercise in workout)
        {
            List<Set> sets = new List<Set>();
            foreach (var set in exercise.Sets)
            {
                sets.Add(new Set
                {
                    Reps = set.Reps,
                    Weight = set.Weight
                });
                volume += set.Reps * set.Weight;
            }
            ExerciseExecution execution = new ExerciseExecution
            {
                UserId = Convert.ToInt32(Request.Cookies["UserId"]),
                ExerciseId = exercise.Id,
                ExecutionDate = DateTime.Now,
                Sets = sets
            };
            exerciseExecutions.Add(execution);
        }
        if (!_exerciseExecutionContainer.AddWorkout(exerciseExecutions))
        {
            return BadRequest("An error occurred while saving the workout.");
        }
        
        // Set number of PR's
        ViewBag.PRs = _exerciseExecutionContainer.GetPRs(exerciseExecutions);
        ViewBag.NumExercises = exerciseExecutions.Count;
        ViewBag.Volume = volume;
        
        return RedirectToAction("Index", "Home");

    }
    
}