using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class LoginController : Controller
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    // GET
    public IActionResult Index()
    {
        return PartialView();
    }
    // POST
    [HttpPost]
    public IActionResult Index(string name, string password)
    {
        // Hash password
        using (SHA256 sha256Hash = SHA256.Create())
        {
            Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            password = builder.ToString();
        }
        
        // Retrieve user from db
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE (username = @name OR LOWER(email) = LOWER(@name)) AND passwordhash = @password", conn);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@password", password);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            // Create cookie with user id
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(365), // Cookie expires after a year
                IsEssential = true
            };
            Response.Cookies.Append("UserId", reader["id"].ToString() ?? throw new InvalidOperationException(), cookieOptions);
            TempData["Id"] = reader["id"];
            TempData["Username"] = reader["username"].ToString();
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }
}