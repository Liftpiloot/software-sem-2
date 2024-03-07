using System.Security.Cryptography;
using System.Text;
using IronApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IronApp.Controllers;

public class RegisterController : Controller
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
    // POST
    [HttpPost]
    public IActionResult Index(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            // Create sha256 hash
            using (SHA256 sha256Hash = SHA256.Create())
            {
                Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                model.Password = builder.ToString();
            }
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO users (username, passwordhash, email, dateofbirth, weight) VALUES (@username, @password, @email, @dateOfBirth, @weight)", conn);
            cmd.Parameters.AddWithValue("@username", model.Username);
            cmd.Parameters.AddWithValue("@password", model.Password);
            cmd.Parameters.AddWithValue("@email", model.Email);
            cmd.Parameters.AddWithValue("@dateOfBirth", model.DateOfBirth);
            cmd.Parameters.AddWithValue("@weight", model.Weight);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return View(model);
        }
        
    }

}