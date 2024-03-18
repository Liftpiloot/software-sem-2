using System.Security.Cryptography;
using System.Text;
using IronApp.Models;
using Microsoft.Data.SqlClient;


namespace IronApp.Classes;

public class User
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";

    public int? Id { get; set; }
    public string? UserName { get; set; }
    private string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? DateOfBirth { get; set; }
    public decimal Weight { get; set; }

    public User()
    {
    }

    public User(string? userName, string? email, string? passwordHash, string? dateOfBirth, decimal weight)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }

    public User(int id, string? userName, string? email, string? passwordHash, string? dateOfBirth, decimal weight)
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
        SqlConnection conn = new SqlConnection(_db);
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

        conn = new SqlConnection(_db);
        conn.Open();
        cmd = new SqlCommand("INSERT INTO users (UserName, PasswordHash, Email, BirthDate, BodyWeight) VALUES (@username, @password, LOWER(@email), @dateOfBirth, @weight); SELECT SCOPE_IDENTITY();", conn);
        cmd.Parameters.AddWithValue("@username", UserName);
        cmd.Parameters.AddWithValue("@password", PasswordHash);
        cmd.Parameters.AddWithValue("@email", Email);
        cmd.Parameters.AddWithValue("@dateOfBirth", DateOfBirth);
        cmd.Parameters.AddWithValue("@weight", Weight);
        int id = Convert.ToInt32(cmd.ExecuteScalar());
        conn.Close();
        this.Id = id;
        return "Success";
    }

    public List<SelectedExercise> GetSelectedExercises()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM selected_exercises WHERE UserID = @userid", conn);
        cmd.Parameters.AddWithValue("@userid", Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<SelectedExercise> selectedExercises = new List<SelectedExercise>();
        while (reader.Read())
        {
            selectedExercises.Add(new SelectedExercise
            {
                UserId = (int)reader["UserID"],
                ExerciseId = (int)reader["ExerciseID"]
            });
        }
        conn.Close();
        reader.Close();
        return selectedExercises;
    }

    public List<ExerciseExecution> GetRecentExercises()
    {
        // get all exercises with unique names, and only show the most recent ones
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT MAX(ExerciseExecutionDate), ExerciseID FROM exercise_executions WHERE UserId=@userid GROUP BY ExerciseID, UserID", conn);
        cmd.Parameters.AddWithValue("@userid", Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<ExerciseExecution> exercises = new List<ExerciseExecution>();
        while (reader.Read())
        {
            exercises.Add(new ExerciseExecution
            {
                Id = (int)reader["ExerciseID"],
                UserId = (int)reader["UserID"]
            });
        }

        conn.Close();
        reader.Close();
        return exercises;
    }

    /// <summary>
    /// Gets all exercises associated with the user, and exercises with no user.
    /// </summary>
    /// <returns>List of exercise</returns>
    public List<Exercise> GetExercises()
    {
        Console.WriteLine("USERID = " + Id.ToString());
        // select all exercises with user id = this.Id, or exercises with user id = null
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE UserID = @userid OR UserID IS NULL", conn);
        cmd.Parameters.AddWithValue("@userid", Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<Exercise> exercises = new List<Exercise>();
        while (reader.Read())
        {
            exercises.Add(new Exercise
            {
                Id = (int)reader["ExerciseID"],
                UserId = reader["UserID"] == DBNull.Value? (int?)null:(int)reader["UserID"],
                Name = reader["ExerciseName"].ToString(),
                Description = reader["ExerciseDescription"].ToString(),
                Logo = reader["LogoFilePath"].ToString()
            });
        }
        conn.Close();
        reader.Close();
        return exercises;
    }
    
    /// <summary>
    /// Returns user on successful login, null otherwise. Either username or email must be set.
    /// </summary>
    /// <returns>User</returns>
    public User? Login()
    {
        // Retrieve user from db
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand(
                "SELECT * FROM users WHERE (UserName = @name OR LOWER(Email) = LOWER(@email)) AND PasswordHash = @password",
                conn);
        cmd.Parameters.AddWithValue("@name", UserName ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@email", Email ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@password", PasswordHash);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            this.Id = (int)reader["UserID"];
            this.DateOfBirth = reader["BirthDate"].ToString();
            this.Email = reader["Email"].ToString();
            this.Weight = (decimal)reader["BodyWeight"];
            reader.Close();

            return this;
        }
        return null;
    }
    
}