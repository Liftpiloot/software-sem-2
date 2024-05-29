using Iron_DAL.DTO;
using Iron_Interface;
using Microsoft.Data.SqlClient;

namespace Iron_DAL;

public class DbExerciseExecution : IDbExerciseExecution
{
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    /// <summary>
    ///  Adds a new exercise execution to the database.
    /// </summary>
    /// <returns>Created execution ID</returns>
    public int AddExerciseExecution(ExerciseExecutionDto exercise)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand(
                    "INSERT INTO exercise_executions (ExerciseExecutionDate ,UserID, ExerciseID) VALUES (GETDATE(), @userid, @exerciseid); SELECT SCOPE_IDENTITY();",
                    conn);
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
        catch (SqlException e)
        {
            return -1;
        }
    }
        
    /// <summary>
    /// Gets the sets of the exercise execution.
    /// </summary>
    /// <returns>List of Set associated with the exercise execution</returns>
    public List<SetDto> GetSets(ExerciseExecutionDto exerciseExecution)
    {
        try
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
        catch (SqlException e)
        {
            return new List<SetDto>();
        }
    }
    
    public bool AddSet(SetDto set)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand(
                    "INSERT INTO exercise_sets (ExerciseExecutionID, SetWeight, SetRepetitions) VALUES (@executionid, @weight, @reps)",
                    conn);
            cmd.Parameters.AddWithValue("@executionid", set.ExerciseExecutionId);
            cmd.Parameters.AddWithValue("@weight", set.Weight);
            cmd.Parameters.AddWithValue("@reps", set.Reps);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            return rows > 0;
        }
        catch (SqlException e)
        {
            return false;
        }
    }
    
    public ExerciseExecutionDto? GetRecentExecution(UserDto user, ExerciseDto exercise)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT * FROM exercise_executions WHERE ExerciseID = @id AND UserID = @userid ORDER BY ExerciseExecutionDate DESC",
                    conn);
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
        catch (SqlException e)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Gets the most recent exercise executions of unique names of a user.
    /// </summary>
    /// <returns>List of exercise executions</returns>
    public List<ExerciseExecutionDto> GetRecentExerciseExecutions(UserDto user)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand(
                    "SELECT MAX(ExerciseExecutionDate), ExerciseID FROM exercise_executions WHERE UserId=@userid GROUP BY ExerciseID, UserID",
                    conn);
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
        catch (SqlException e)
        {
            return new List<ExerciseExecutionDto>();
        }
    }
    
    // get all executions of a user of one exercise
    public List<ExerciseExecutionDto> GetExerciseExecutions(UserDto user, ExerciseDto exercise)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd =
                new SqlCommand("SELECT * FROM exercise_executions WHERE UserID = @userid AND ExerciseID = @exerciseid",
                    conn);
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
        catch (SqlException e)
        {
            return new List<ExerciseExecutionDto>();
        }
    }

    public bool IsPersonalBest(ExerciseExecutionDto exerciseExecutionDto, List<SetDto> setDtos)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            // Select all executions where exercise id is the same
            SqlCommand cmd =
                new SqlCommand("SELECT * FROM exercise_executions WHERE ExerciseID = @exerciseid AND UserID = @userid",
                    conn);
            cmd.Parameters.AddWithValue("@exerciseid", exerciseExecutionDto.ExerciseId);
            cmd.Parameters.AddWithValue("@userid", exerciseExecutionDto.UserId);
            SqlDataReader reader = cmd.ExecuteReader();
            List<int> executionIds = new List<int>();
            while (reader.Read())
            {
                executionIds.Add((int)reader["ExerciseExecutionID"]);
            }

            reader.Close();

            List<SetDto> allSets = new List<SetDto>();
            foreach (var executionId in executionIds)
            {
                // Get all sets for each execution
                SqlCommand setCmd =
                    new SqlCommand("SELECT * FROM exercise_sets WHERE ExerciseExecutionID = @executionid", conn);
                setCmd.Parameters.AddWithValue("@executionid", executionId);
                SqlDataReader setReader = setCmd.ExecuteReader();
                while (setReader.Read())
                {
                    allSets.Add(new SetDto
                    {
                        ExerciseExecutionId = (int)setReader["ExerciseExecutionID"],
                        Reps = (int)setReader["SetRepetitions"],
                        Weight = (decimal)setReader["SetWeight"]
                    });
                }

                setReader.Close();
            }

            conn.Close();
            // Check if the new set is the best
            return allSets.Count == 0 || allSets.Max(set => set.Weight) < setDtos.Max(set => set.Weight);
        }
        catch (SqlException e)
        {
            return false;
        }
    }

    public List<ExerciseExecutionDto> GetAllExecutionsForUser(int userId)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM exercise_executions WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", userId);
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
        catch (SqlException e)
        {
            return new List<ExerciseExecutionDto>();
        }
    }
}