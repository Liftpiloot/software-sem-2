using System.Diagnostics;
using Iron_DAL;
using Microsoft.AspNetCore.Mvc;
using IronApp.Models;
using Iron_Domain;
using Iron_Interface;
using IronDomain;
using Microsoft.AspNetCore.SignalR;

namespace IronApp.Controllers;

public class ExerciseController : Controller
{
    private readonly ExerciseContainer _exerciseContainer;
    private readonly ExerciseExecutionContainer _exerciseExecutionContainer;
    private readonly IDbExercise _dbExercise;
    private readonly IDbExerciseExecution _dbExerciseExecution;
    private readonly UserContainer _userContainer;
    private readonly IDbUser _dbUser;
    
    public ExerciseController()
    {
        _dbExercise = new DbExercise();
        _dbExerciseExecution = new DbExerciseExecution();
        _exerciseContainer = new ExerciseContainer(_dbExercise);
        _exerciseExecutionContainer = new ExerciseExecutionContainer(_dbExerciseExecution);
        _dbUser = new DbUser();
        _userContainer = new UserContainer(_dbUser);
    }
    
    public IActionResult Index()
    {
        bool isLoggedIn = int.TryParse(Request.Cookies["UserId"], out var userId);
        if (!isLoggedIn)
        {
            return RedirectToAction("Index", "Login");
        }
        User? user = _userContainer.GetUser(userId);
        if (user == null)
        {
            return RedirectToAction("Index", "Login");
        }
        // get selected exercises, and add sets if there is an execution. Create a List of ExerciseModel to pass to the view

        var selectedExercises = _exerciseContainer.GetSelectedExercises(user);
        var exerciseModels = GetRecentExecutionsAndSets(selectedExercises, user);
        return View(exerciseModels);
    }

    private List<ExerciseModel> GetRecentExecutionsAndSets(List<SelectedExercise> selectedExercises, User user)
    {
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

            var recentExecution = _exerciseExecutionContainer.GetRecentExerciseExecution(user, exercise);
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

        return exerciseModels;
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
        User? user = _userContainer.GetUser(userId);
        if (user == null)
        {
            return RedirectToAction("Index", "Login");
        }
        List<ExerciseModel> exerciseModels = new List<ExerciseModel>();
        List<Exercise> exercises = _exerciseContainer.GetUnselectedExercises(user);
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
        int userId = Convert.ToInt32(Request.Cookies["UserId"]);
        SelectedExercise selectedExercise = new SelectedExercise(userId, id);
        _exerciseContainer.AddSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Exercise");
    }

    public IActionResult? Exercise(int id)
    {
        // Get exercise information
        Exercise? exercise = _exerciseContainer.GetExerciseFromId(id);
        if (exercise != null)
        {
            ExerciseModel exerciseModel = new ExerciseModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Logo = exercise.Logo
            };
            
            // Get user information
            var userId = Convert.ToInt32(Request.Cookies["UserId"]);
            User? user = _userContainer.GetUser(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            var allSets = GetAllSets(user, exercise);
            exerciseModel = GetExerciseGraphData(allSets, exerciseModel);
            return View(exerciseModel);
        }

        return null;
    }

    private List<List<SetModel>> GetAllSets(User user, Exercise exercise)
    {
        List<List<SetModel>> allSets = new List<List<SetModel>>();
        List<ExerciseExecution> exerciseExecutions = _exerciseExecutionContainer.GetExerciseExecutions(user, exercise);
        foreach (ExerciseExecution exerciseExecution in exerciseExecutions)
        {
            List<Set> executionSets = _exerciseExecutionContainer.GetSets(exerciseExecution);
            if (executionSets.Count != 0)
            {
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
        }
        return allSets;
    }

    private static ExerciseModel GetExerciseGraphData(List<List<SetModel>> allSets, ExerciseModel exerciseModel)
    {
        List<DataPointModel> highestWeightOverTime = new List<DataPointModel>();
        List<DataPointModel> volumeOverTime = new List<DataPointModel>();
        foreach (List<SetModel> sets in allSets)
        {
            // Get the highest weight
            decimal highestWeight = sets.Select(set => set.Weight).Prepend(0).Max();
            // Get the volume
            decimal volume = sets.Select(set => set.Reps * set.Weight).Sum();
            // Get date
            DateTime date = sets[0].Date;
            DataPointModel dataPoint = new DataPointModel(date, highestWeight);
            DataPointModel dataPoint2 = new DataPointModel(date, volume);
            highestWeightOverTime.Add(dataPoint);
            volumeOverTime.Add(dataPoint2);
        }
        exerciseModel.highestWeightOverTime = highestWeightOverTime;
        exerciseModel.volumeOverTime = volumeOverTime;
        return exerciseModel;
    }

    public IActionResult DeleteExercise(int id)
    {
        var userId = Convert.ToInt32(Request.Cookies["UserId"]);
        if (userId == 0)
        {
            return RedirectToAction("Index", "Login");
        }
        SelectedExercise selectedExercise = new SelectedExercise(userId, id);
        _exerciseContainer.DeleteSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Exercise");
    }

    [HttpPost]
    public IActionResult AddCustomExercise(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return ExerciseList();
        }
        var userId = Convert.ToInt32(Request.Cookies["UserId"]);
        if (userId == 0)
        {
            return RedirectToAction("Index", "Login");
        }
        var exercise = new Exercise(0, userId, name, name, "/images/default.png");
        var exerciseId = _exerciseContainer.AddExercise(exercise);
        if (exerciseId <= 0) return ExerciseList();
        SelectedExercise selectedExercise = new SelectedExercise(userId, exerciseId);
        _exerciseContainer.AddSelectedExercise(selectedExercise);
        return RedirectToAction("Index", "Exercise");
    }

    [HttpPost]
    public IActionResult SaveWorkout([FromBody] List<WorkoutEntryModel> workout)
    {
        if (workout.Count == 0)
        {
            return BadRequest("No exercises in workout");
        }
        
        var userId = Convert.ToInt32(Request.Cookies["UserId"]);
        if (userId == 0)
        {
            return RedirectToAction("Index", "Login");
        }

        decimal volume = 0;
        List<ExerciseExecution> personalBests = new List<ExerciseExecution>();
        List<ExerciseExecution> exerciseExecutions = new List<ExerciseExecution>();
        foreach (var exercise in workout)
        {
            List<Set> sets = new List<Set>();
            foreach (var set in exercise.Sets)
            {
                sets.Add(new Set(set.Weight, set.Reps));
                volume += set.Reps * set.Weight;
            }
            ExerciseExecution execution = new ExerciseExecution(0, DateTime.Now, userId, exercise.Id, sets);
            exerciseExecutions.Add(execution);
            bool isPersonalBest = _exerciseExecutionContainer.IsPersonalBest(execution, sets);
            if (isPersonalBest)
            {
                personalBests.Add(execution);
            }
            
        }
        if (!_exerciseExecutionContainer.AddWorkout(exerciseExecutions))
        {
            return BadRequest("An error occurred while saving the workout.");
        }
        
        return Json(new { numexercises = exerciseExecutions.Count, volume = volume, prs = personalBests.Count });
    }
    
}