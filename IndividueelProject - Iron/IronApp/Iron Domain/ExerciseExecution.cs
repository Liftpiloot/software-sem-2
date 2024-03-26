namespace Iron_Domain
{
    public class ExerciseExecution
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int ExerciseId { get; set; }

        private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
        
        
        /// <summary>
        ///  Adds a new exercise execution to the database.
        /// </summary>
        /// <returns>Created execution ID</returns>
        public int AddExerciseExecution()
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO exercise_executions (ExerciseExecutionDate ,UserID, ExerciseID) VALUES (GETDATE(), @userid, @exerciseid); SELECT SCOPE_IDENTITY();", conn);
            cmd.Parameters.AddWithValue("@userid", UserId);
            cmd.Parameters.AddWithValue("@exerciseid", ExerciseId);
            int id = Convert.ToInt32((object?)cmd.ExecuteScalar());
            conn.Close();
            return id;
        }
        
        /// <summary>
        /// Gets the sets of the exercise execution.
        /// </summary>
        /// <returns>List of Set associated with the exercise execution</returns>
        public List<Set> GetSets()
        {
            SqlConnection conn = new SqlConnection(_db);
            conn.Open();
            List<Set> sets = new List<Set>();
            SqlCommand cmd = new SqlCommand($"SELECT * from exercise_sets WHERE ExerciseExecutionID = @id", conn);
            cmd.Parameters.AddWithValue("@id", Id);
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
        
    }
}