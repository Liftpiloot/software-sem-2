using Microsoft.Data.SqlClient;

namespace IronDomain;

public class SelectedExercise
{
    public int UserId { get; set; }
    public int ExerciseId { get; set; }

    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    public void AddSelectedExercise()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand(
                "INSERT INTO selected_exercises (UserID, ExerciseID) VALUES (@userid, @exerciseid)",
                conn);
        cmd.Parameters.AddWithValue("@userid", UserId);
        cmd.Parameters.AddWithValue("@exerciseid", ExerciseId);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void RemoveSelectedExercise()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM selected_exercises WHERE UserID = @userid AND ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@userid", UserId);
        cmd.Parameters.AddWithValue("@exercisetypeid", ExerciseId);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public Exercise? GetExercise()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd;
        cmd = new SqlCommand("SELECT * FROM exercises WHERE ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@exerciseid", ExerciseId);
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

    public void DeleteSelectedExercise()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM selected_exercises WHERE UserID = @userid AND ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@userid", UserId);
        cmd.Parameters.AddWithValue("@exerciseid", ExerciseId);
        cmd.ExecuteNonQuery();
        conn.Close();
    }
}