using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IronApp.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

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
        }
        return View();
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
}