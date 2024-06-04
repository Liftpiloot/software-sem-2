using Iron_DAL.DTO;
using Iron_Interface;
using Microsoft.Data.SqlClient;
namespace Iron_DAL;

public class DbExercise : IDbExercise
{
    private readonly string _db = "data source=P-STUDSQL02;initial catalog=dbi538068_iron;user id=dbi538068_iron;password=Dungprotfi8;TrustServerCertificate=True";
    
    public int AddExercise(ExerciseDto exercise)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            // Check if the exercise already exists.
            using SqlCommand checkCmd =
                new SqlCommand(
                    "SELECT COUNT(*) FROM exercises WHERE ExerciseName = @name AND UserID = @userid OR UserID IS NULL AND ExerciseName = @name",
                    conn);
            checkCmd.Parameters.AddWithValue("@name", exercise.Name);
            checkCmd.Parameters.AddWithValue("@userid", exercise.UserId);
            var count = (int)checkCmd.ExecuteScalar();
            if (count > 0)
            {
                conn.Close();
                return 0;
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
        catch (SqlException e)
        {
            return -1;
        }
    }
    

    /// <summary>
    /// Retrieve exercise information from the database, given the exercise id.
    /// </summary>
    /// <param name="exercise">Exercise object with only ID added.</param>
    /// <returns>Exercise</returns>
    public ExerciseDto? GetExercise(ExerciseDto exercise)
    {
        try
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
                    Name = reader["ExerciseName"].ToString() ?? string.Empty,
                    Description = reader["ExerciseDescription"].ToString() ?? string.Empty,
                    Logo = reader["LogoFilePath"].ToString() ?? string.Empty
                };
                conn.Close();
                return returnExercise;
            }
            conn.Close();
            return null;
        }
        catch (SqlException e)
        {
            return null;
        }
    }
    
    public bool AddSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        try
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
        catch (SqlException e)
        {
            return false;
        }
    }

    public bool DeleteSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand("DELETE FROM selected_exercises WHERE UserID = @userid AND ExerciseID = @exerciseid",
                    conn);
            cmd.Parameters.AddWithValue("@userid", selectedExerciseDto.UserId);
            cmd.Parameters.AddWithValue("@exerciseid", selectedExerciseDto.ExerciseId);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            return rows > 0;
        }
        catch (SqlException e)
        {
            return false;
        }
    }
    
    /// <summary>
    /// Gets all selected exercises of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of SelectedExercise</returns>
    public List<SelectedExerciseDto> GetSelectedExercises(UserDto user)
    {
        List<SelectedExerciseDto> selectedExercises = new();
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM selected_exercises WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", user.Id);
            SqlDataReader reader = cmd.ExecuteReader();
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
        }
        catch (SqlException e)
        {
        }
        return selectedExercises;
    }

    public List<ExerciseDto> GetUnselectedExercises(UserDto user)
    {
        List<ExerciseDto> exercises = new();
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT exercises.* " +
                "FROM exercises " +
                "LEFT JOIN selected_exercises ON exercises.ExerciseID = selected_exercises.ExerciseID AND selected_exercises.UserID = @userid " +
                "WHERE selected_exercises.UserID IS NULL AND selected_exercises.ExerciseID IS NULL AND (exercises.UserID = @userid OR exercises.UserID IS NULL);",
                conn);
            cmd.Parameters.AddWithValue("@userid", user.Id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exercises.Add(new ExerciseDto
                {
                    Id = (int)reader["ExerciseID"],
                    UserId = reader["UserID"] == DBNull.Value ? null : (int)reader["UserID"],
                    Name = reader["ExerciseName"].ToString() ?? string.Empty,
                    Description = reader["ExerciseDescription"].ToString() ?? string.Empty,
                    Logo = reader["LogoFilePath"].ToString() ?? string.Empty
                });
            }

            conn.Close();
            reader.Close();
        }
        catch (SqlException e)
        {
        }
        return exercises;
    }


    
    /// <summary>
    /// Gets all exercises associated with the user, and exercises with no user.
    /// </summary>
    /// <returns>List of exercise</returns>
    public List<ExerciseDto> GetExercises(UserDto user)
    {
        List<ExerciseDto> exercises = new();
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE UserID = @userid OR UserID IS NULL", conn);
            cmd.Parameters.AddWithValue("@userid", user.Id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exercises.Add(new ExerciseDto
                {
                    Id = (int)reader["ExerciseID"],
                    UserId = reader["UserID"] == DBNull.Value ? (int?)null : (int)reader["UserID"],
                    Name = reader["ExerciseName"].ToString() ?? string.Empty,
                    Description = reader["ExerciseDescription"].ToString() ?? string.Empty,
                    Logo = reader["LogoFilePath"].ToString() ?? string.Empty
                });
            }

            conn.Close();
            reader.Close();
        }
        catch (SqlException e)
        {
        }
        return exercises;
    }

    public bool DeleteExercise(UserDto userDto, ExerciseDto exerciseDto)
    {
        try
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
        catch (SqlException e)
        {
            return false;
        }
    }
}