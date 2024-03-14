using System.Runtime.InteropServices.JavaScript;
using IronApp.Models;
using Microsoft.Data.SqlClient;

namespace IronApp.Classes;

public class User
{
    private string db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";

    public int Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public JSType.Date DateOfBirth { get; set; }
    public decimal Weight { get; set; }

    public User(int id, string userName, string passwordHash, JSType.Date dateOfBirth, decimal weight)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }

    public void AddUser()
    {
        // TODO
        // Add user to database
    }

    public void GetRecentExercises()
    {
        // TODO
        // Get recent exercises from database
    }

    public void GetExercises()
    {
        // TODO
        // Get exercises from database
    }

    public List<CustomExercise> GetCustomExercises()
    {
        SqlConnection conn = new SqlConnection(db);
        conn.Open();
        
        SqlCommand cmd = new SqlCommand("SELECT * FROM custom_exercises WHERE UserId = @userId", conn);
        cmd.Parameters.AddWithValue("@userId", Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<CustomExercise> exercises = new List<CustomExercise>();
        while (reader.Read())
        {
            exercises.Add(new CustomExercise()
            {
                Id = (int)reader["CustomExerciseID"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
            });
        }
        conn.Close();
        reader.Close();
        return exercises;
    }

    public List<PreDefinedExercise> GetPreDefinedExercises()
    {
        SqlConnection conn = new SqlConnection(db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM predefined_exercises", conn);
        SqlDataReader reader = cmd.ExecuteReader();
        List<PreDefinedExercise> exercises = new List<PreDefinedExercise>();
        while (reader.Read())
        {
            exercises.Add(new PreDefinedExercise
            {
                Id = (int)reader["PreDefinedExerciseID"],
                Name = reader["Name"].ToString(),
                Description= reader["Description"].ToString(),
                Logo = reader["Logo"].ToString(),
            });
        }
        conn.Close();
        reader.Close();
        return exercises;
    }
}