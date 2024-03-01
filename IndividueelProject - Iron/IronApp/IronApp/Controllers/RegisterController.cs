using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class RegisterController : Controller
{
    private string db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
    // POST
    [HttpPost]
    public IActionResult Index(string username, string password, string email, int age, decimal weight)
    {
        // Create sha256 hash
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
        SqlConnection conn = new SqlConnection(db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO users (username, password, email, age, weight) VALUES (@username, @password, @email, @age, @weight)", conn);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@age", age);
        cmd.Parameters.AddWithValue("@weight", weight);
        cmd.ExecuteNonQuery();
        return RedirectToAction("Index", "Home");
    }

}