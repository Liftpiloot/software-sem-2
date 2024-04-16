using Iron_DAL.DTO;
using Microsoft.Data.SqlClient;
namespace Iron_DAL;

public class DbExercise
{
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    public int AddExercise(ExerciseDto exercise)
    {
        using (SqlConnection conn = new SqlConnection(_db))
        {
            conn.Open();
            // Check if the exercise already exists.
            using SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM exercises WHERE ExerciseName = @name AND UserID = @userid OR UserID IS NULL AND ExerciseName = @name", conn);
            checkCmd.Parameters.AddWithValue("@name", exercise.Name);
            checkCmd.Parameters.AddWithValue("@userid", exercise.UserId);
            var count = (int)checkCmd.ExecuteScalar();
            if (count > 0)
            {
                conn.Close();
                return -1;
            }
            // Add the exercise to the database.
            using SqlCommand cmd =
                new SqlCommand(
                    "INSERT INTO exercises (UserID, ExerciseName, ExerciseDescription, LogoFilePath) VALUES (@userid, @name, @description, @logo); SELECT SCOPE_IDENTITY();",
                    conn);
            cmd.Parameters.AddWithValue("@userid", exercise.UserId);
            cmd.Parameters.AddWithValue("@name", exercise.Name);
            cmd.Parameters.AddWithValue("@description", exercise.Description);
            cmd.Parameters.AddWithValue("@logo", exercise.Logo);
            var id = Convert.ToInt32((object?)cmd.ExecuteScalar());
            conn.Close();
            return id;
        }
    }
    

    /// <summary>
    /// Retrieve exercise information from the database, given the exercise id.
    /// </summary>
    /// <param name="exercise">Exercise object with only ID added.</param>
    /// <returns>Exercise</returns>
    public ExerciseDto? GetExercise(ExerciseDto exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE ExerciseID = @id", conn);
        cmd.Parameters.AddWithValue("@id", exercise.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ExerciseDto? returnExercise = new ExerciseDto
            {
                Id = (int)reader["ExerciseID"],
                UserId = reader["UserID"] == DBNull.Value ? null : (int)reader["UserID"],
                Name = reader["ExerciseName"].ToString(),
                Description = reader["ExerciseDescription"].ToString(),
                Logo = reader["LogoFilePath"].ToString()
            };
            conn.Close();
            return returnExercise;
        }
        conn.Close();
        return null;
    }
    
    public bool AddSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand(
                "INSERT INTO selected_exercises (UserID, ExerciseID) VALUES (@userid, @exerciseid)",
                conn);
        cmd.Parameters.AddWithValue("@userid", selectedExerciseDto.UserId);
        cmd.Parameters.AddWithValue("@exerciseid", selectedExerciseDto.ExerciseId);
        int rows = cmd.ExecuteNonQuery();
        conn.Close();
        return rows > 0;
    }

    public bool DeleteSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM selected_exercises WHERE UserID = @userid AND ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@userid", selectedExerciseDto.UserId);
        cmd.Parameters.AddWithValue("@exerciseid", selectedExerciseDto.ExerciseId);
        int rows = cmd.ExecuteNonQuery();
        conn.Close();
        return rows > 0;
    }
    
    /// <summary>
    /// Gets all selected exercises of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of SelectedExercise</returns>
    public List<SelectedExerciseDto> GetSelectedExercises(UserDto user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand(
            "SELECT * FROM selected_exercises WHERE UserID = @userid", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<SelectedExerciseDto> selectedExercises = new List<SelectedExerciseDto>();
        while (reader.Read())
        {
            selectedExercises.Add(new SelectedExerciseDto
            {
                UserId = (int)reader["UserID"],
                ExerciseId = (int)reader["ExerciseID"]
            });
        }
        conn.Close();
        reader.Close();
        return selectedExercises;
    }

    public List<ExerciseDto> GetUnselectedExercises(UserDto user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand(
    "SELECT exercises.* " +
            "FROM exercises " +
            "LEFT JOIN selected_exercises ON exercises.ExerciseID = selected_exercises.ExerciseID AND selected_exercises.UserID = @userid " +
            "WHERE selected_exercises.UserID IS NULL AND selected_exercises.ExerciseID IS NULL AND (exercises.UserID = @userid OR exercises.UserID IS NULL);", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<ExerciseDto> exercises = new List<ExerciseDto>();
        while (reader.Read())
        {
            exercises.Add(new ExerciseDto
            {
                Id = (int)reader["ExerciseID"],
                UserId = reader["UserID"] == DBNull.Value ? null : (int)reader["UserID"],
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
    /// Gets all exercises associated with the user, and exercises with no user.
    /// </summary>
    /// <returns>List of exercise</returns>
    public List<ExerciseDto> GetExercises(UserDto user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE UserID = @userid OR UserID IS NULL", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<ExerciseDto> exercises = new List<ExerciseDto>();
        while (reader.Read())
        {
            exercises.Add(new ExerciseDto
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

    public bool DeleteExercise(UserDto userDto, ExerciseDto exerciseDto)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM exercises WHERE ExerciseID = @id AND UserID = @userid", conn);
        cmd.Parameters.AddWithValue("@id", exerciseDto.Id);
        cmd.Parameters.AddWithValue("@userid", userDto.Id);
        int rows = cmd.ExecuteNonQuery();
        conn.Close();
        return rows > 0;
    }
}