using Iron_Domain;
using Microsoft.Data.SqlClient;

namespace Iron_DAL;

public class DbExercise
{
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    /// <summary>
    ///  Adds a new exercise execution to the database.
    /// </summary>
    /// <returns>Created execution ID</returns>
    public int AddExerciseExecution(User user, Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercise_executions (ExerciseExecutionDate ,UserID, ExerciseID) VALUES (GETDATE(), @userid, @exerciseid); SELECT SCOPE_IDENTITY();", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@exerciseid", exercise.Id);
        int id = Convert.ToInt32((object?)cmd.ExecuteScalar());
        conn.Close();
        return id;
    }
        
    /// <summary>
    /// Gets the sets of the exercise execution.
    /// </summary>
    /// <returns>List of Set associated with the exercise execution</returns>
    public List<Set> GetSets(ExerciseExecution exerciseExecution)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        List<Set> sets = new List<Set>();
        SqlCommand cmd = new SqlCommand($"SELECT * from exercise_sets WHERE ExerciseExecutionID = @id", conn);
        cmd.Parameters.AddWithValue("@id", exerciseExecution.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            sets.Add(new Set()
            {
                Weight = (decimal)reader["SetWeight"],
                Reps = (int)reader["SetRepetitions"]
            });
        }
        reader.Close();
        conn.Close();
        return sets;
    }
    
    public int AddExercise(User user, Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercises (UserID, ExerciseName, ExerciseDescription, LogoFilePath) VALUES (@userid, @name, @description, @logo); SELECT SCOPE_IDENTITY();", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@name", exercise.Name);
        cmd.Parameters.AddWithValue("@description", exercise.Description);
        cmd.Parameters.AddWithValue("@logo", exercise.Logo);
        int id = Convert.ToInt32((object?)cmd.ExecuteScalar());
        conn.Close();
        return id;
    }

    public ExerciseExecution? GetRecentExecution(User user, Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercise_executions WHERE ExerciseID = @id AND UserID = @userid ORDER BY ExerciseExecutionDate DESC", conn);
        cmd.Parameters.AddWithValue("@id", exercise.Id);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ExerciseExecution exerciseExecution = new ExerciseExecution
            {
                Id = (int)reader["ExerciseExecutionID"],
                ExerciseId = (int)reader["ExerciseID"]
            };
            conn.Close();
            return exerciseExecution;
        }
        conn.Close();
        return null;
    }

    /// <summary>
    /// Retrieve exercise information from the database, given the exercise id.
    /// </summary>
    /// <param name="exercise">Exercise object with only ID added.</param>
    /// <returns>Exercise</returns>
    public Exercise? GetExercise(Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE ExerciseID = @id", conn);
        cmd.Parameters.AddWithValue("@id", exercise.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Exercise? returnExercise = new Exercise
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
    
    public void AddSelectedExercise(User user, Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd =
            new SqlCommand(
                "INSERT INTO selected_exercises (UserID, ExerciseID) VALUES (@userid, @exerciseid)",
                conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@exerciseid", exercise.Id);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void RemoveSelectedExercise(User user, Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM selected_exercises WHERE UserID = @userid AND ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@exerciseid", exercise.Id);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void DeleteSelectedExercise(User user, Exercise exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM selected_exercises WHERE UserID = @userid AND ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@exerciseid", exercise.Id);
        cmd.ExecuteNonQuery();
        conn.Close();
    }
    
    /// <summary>
    /// Gets all selected exercises of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of SelectedExercise</returns>
    public List<SelectedExercise> GetSelectedExercises(User user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM selected_exercises WHERE UserID = @userid", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
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

    /// <summary>
    /// Gets the most recent exercise executions of unique names of a user.
    /// </summary>
    /// <returns>List of exercise executions</returns>
    public List<ExerciseExecution> GetRecentExercises(User user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT MAX(ExerciseExecutionDate), ExerciseID FROM exercise_executions WHERE UserId=@userid GROUP BY ExerciseID, UserID", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
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
    public List<Exercise> GetExercises(User user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE UserID = @userid OR UserID IS NULL", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
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
}