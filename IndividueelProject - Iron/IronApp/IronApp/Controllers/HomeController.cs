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
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", Request.Cookies["UserId"]);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ViewBag.Username = reader["username"].ToString() ?? "No username";
            }
            reader.Close();
        }
        Exercise exercise = new Exercise();
        int userid = Convert.ToInt32(Request.Cookies["UserId"]);
        return View(exercise.GetRecentExercises(userid));
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
        ExerciseDefinitions exerciseDefinitions = new ExerciseDefinitions();
        List<ExerciseModel> exercises = exerciseDefinitions.GetExercises();
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