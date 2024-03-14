using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using IronApp.Models;
using Microsoft.Data.SqlClient;

namespace IronApp.Classes;

public class User
{
    private string db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";

    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string DateOfBirth { get; set; }
    public decimal Weight { get; set; }

    public User()
    {
    }

    public User(string userName, string email, string passwordHash, string dateOfBirth, decimal weight)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }

    public User(int id, string userName, string email, string passwordHash, string dateOfBirth, decimal weight)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }

    public string AddUser()
    {
        // check if username or email already exists
        SqlConnection conn = new SqlConnection(db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand("SELECT * FROM users WHERE username = @username OR LOWER(email) = LOWER(@email)", conn);
        cmd.Parameters.AddWithValue("@username", UserName);
        cmd.Parameters.AddWithValue("@email", Email);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return "Username or email already exists";
        }

        // Create sha256 hash
        using (SHA256 sha256Hash = SHA256.Create())
        {
            Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordHash));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            PasswordHash = builder.ToString();
        }

        conn = new SqlConnection(db);
        conn.Open();
        cmd = new SqlCommand(
            "INSERT INTO users (username, passwordhash, email, dateofbirth, weight) OUTPUT INSERTED.id VALUES (@username, @password, @email, @dateOfBirth, @weight)",
            conn);
        cmd.Parameters.AddWithValue("@username", UserName);
        cmd.Parameters.AddWithValue("@password", PasswordHash);
        cmd.Parameters.AddWithValue("@email", Email);
        cmd.Parameters.AddWithValue("@dateOfBirth", DateOfBirth);
        cmd.Parameters.AddWithValue("@weight", Weight);
        int id = (int)cmd.ExecuteScalar();
        conn.Close();
        this.Id = id;
        return "Success";
    }

    public List<Exercise> GetRecentExercises()
    {
        // get all exercises with unique names, and only show the most recent ones
        SqlConnection conn = new SqlConnection(db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT Name, max(Date), ExerciseTypeID, ExerciseType, ExerciseID FROM Exercises WHERE UserId=@userid GROUP BY Name, ExerciseTypeID, ExerciseType, ExerciseID", conn);
        cmd.Parameters.AddWithValue("@userid", Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<Exercise> exercises = new List<Exercise>();
        while (reader.Read())
        {
            exercises.Add(new Exercise
            {
                Id = (int)reader["ExerciseID"],
                Name = reader["Name"].ToString(),
                ExerciseTypeId = Convert.ToInt32(reader["ExerciseTypeId"]),
                Type = (ExerciseType)Enum.Parse(typeof(ExerciseType), reader["ExerciseType"].ToString())
            });
        }

        conn.Close();
        reader.Close();
        return exercises;
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
                Description = reader["Description"].ToString(),
                Logo = reader["Logo"].ToString(),
            });
        }

        conn.Close();
        reader.Close();
        return exercises;
    }

    public User Login()
    {
        // Retrieve user from db
        SqlConnection conn = new SqlConnection(db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand(
                "SELECT * FROM users WHERE (username = @name OR LOWER(email) = LOWER(@name)) AND passwordhash = @password",
                conn);
        cmd.Parameters.AddWithValue("@name", UserName);
        cmd.Parameters.AddWithValue("@password", PasswordHash);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            this.Id = (int)reader["id"];
            this.DateOfBirth = reader["dateofbirth"].ToString();
            this.Email = reader["email"].ToString();
            this.Weight = (decimal)reader["weight"];
            reader.Close();

            return this;
        }

        return null;
    }
}