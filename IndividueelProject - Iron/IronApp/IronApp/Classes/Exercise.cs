using Microsoft.Data.SqlClient;

namespace IronApp.Classes;

public class Exercise
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    public int AddExercise()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercises (UserID, ExerciseName, ExerciseDescription, LogoFilePath) VALUES (@userid, @name, @description, @logo); SELECT SCOPE_IDENTITY();", conn);
        cmd.Parameters.AddWithValue("@userid", UserId);
        cmd.Parameters.AddWithValue("@name", Name);
        cmd.Parameters.AddWithValue("@description", Description);
        cmd.Parameters.AddWithValue("@logo", Logo);
        int id = Convert.ToInt32(cmd.ExecuteScalar());
        conn.Close();
        return id;
    }

    public ExerciseExecution? GetRecentExecution(int userId)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercise_executions WHERE ExerciseID = @id AND UserID = @userid ORDER BY ExerciseExecutionDate DESC", conn);
        cmd.Parameters.AddWithValue("@id", Id);
        cmd.Parameters.AddWithValue("@userid", userId);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ExerciseExecution exerciseExecution = new ExerciseExecution
            {
                Id = (int)reader["ExerciseExecutionID"],
                ExerciseId = (int)reader["ExerciseID"]
            };
            return exerciseExecution;
        }
        return null;
    }

    public Exercise GetExercise()
    {
        // get the exercise with the given id
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE ExerciseID = @id", conn);
        cmd.Parameters.AddWithValue("@id", Id);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Exercise exercise = new Exercise
            {
                Id = (int)reader["ExerciseID"],
                UserId = reader["UserID"] == DBNull.Value ? null : (int)reader["UserID"],
                Name = reader["ExerciseName"].ToString(),
                Description = reader["ExerciseDescription"].ToString(),
                Logo = reader["LogoFilePath"].ToString()
            };
            return exercise;
        }
        return null;
    }
    
}