using Iron_DAL.DTO;
using Microsoft.Data.SqlClient;

namespace Iron_DAL;

public class DbExerciseExecution
{
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    /// <summary>
    ///  Adds a new exercise execution to the database.
    /// </summary>
    /// <returns>Created execution ID</returns>
    public int AddExerciseExecution(ExerciseExecutionDto exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercise_executions (ExerciseExecutionDate ,UserID, ExerciseID) VALUES (GETDATE(), @userid, @exerciseid); SELECT SCOPE_IDENTITY();", conn);
        cmd.Parameters.AddWithValue("@userid", exercise.UserId);
        cmd.Parameters.AddWithValue("@exerciseid", exercise.ExerciseId);
        var result = cmd.ExecuteScalar();
        if (result == null)
        {
            return -1;
        }
        var id = Convert.ToInt32(result);
        conn.Close();
        return id;
    }
        
    /// <summary>
    /// Gets the sets of the exercise execution.
    /// </summary>
    /// <returns>List of Set associated with the exercise execution</returns>
    public List<SetDto> GetSets(ExerciseExecutionDto exerciseExecution)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        List<SetDto> sets = new List<SetDto>();
        SqlCommand cmd = new SqlCommand($"SELECT * from exercise_sets WHERE ExerciseExecutionID = @id", conn);
        cmd.Parameters.AddWithValue("@id", exerciseExecution.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            sets.Add(new SetDto()
            {
                Weight = (decimal)reader["SetWeight"],
                Reps = (int)reader["SetRepetitions"]
            });
        }
        reader.Close();
        conn.Close();
        return sets;
    }
    
    public bool AddSet(SetDto set)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercise_sets (ExerciseExecutionID, SetWeight, SetRepetitions) VALUES (@executionid, @weight, @reps)", conn);
        cmd.Parameters.AddWithValue("@executionid", set.ExerciseExecutionId);
        cmd.Parameters.AddWithValue("@weight", set.Weight);
        cmd.Parameters.AddWithValue("@reps", set.Reps);
        int rows = cmd.ExecuteNonQuery();
        conn.Close();
        return rows > 0;
    }
    
    public ExerciseExecutionDto? GetRecentExecution(UserDto user, ExerciseDto exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercise_executions WHERE ExerciseID = @id AND UserID = @userid ORDER BY ExerciseExecutionDate DESC", conn);
        cmd.Parameters.AddWithValue("@id", exercise.Id);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ExerciseExecutionDto exerciseExecution = new ExerciseExecutionDto
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
    /// Gets the most recent exercise executions of unique names of a user.
    /// </summary>
    /// <returns>List of exercise executions</returns>
    public List<ExerciseExecutionDto> GetRecentExerciseExecutions(UserDto user)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT MAX(ExerciseExecutionDate), ExerciseID FROM exercise_executions WHERE UserId=@userid GROUP BY ExerciseID, UserID", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<ExerciseExecutionDto> exercises = new List<ExerciseExecutionDto>();
        while (reader.Read())
        {
            exercises.Add(new ExerciseExecutionDto
            {
                Id = (int)reader["ExerciseID"],
                UserId = (int)reader["UserID"]
            });
        }
        conn.Close();
        reader.Close();
        return exercises;
    }
    
    // get all executions of a user of one exercise
    public List<ExerciseExecutionDto> GetExerciseExecutions(UserDto user, ExerciseDto exercise)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercise_executions WHERE UserID = @userid AND ExerciseID = @exerciseid", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@exerciseid", exercise.Id);
        SqlDataReader reader = cmd.ExecuteReader();
        List<ExerciseExecutionDto> executions = new List<ExerciseExecutionDto>();
        while (reader.Read())
        {
            executions.Add(new ExerciseExecutionDto
            {
                Id = (int)reader["ExerciseExecutionID"],
                UserId = (int)reader["UserID"],
                ExerciseId = (int)reader["ExerciseID"],
                ExecutionDate = (DateTime)reader["ExerciseExecutionDate"]
            });
        }
        conn.Close();
        reader.Close();
        return executions;
    }
    
    
}