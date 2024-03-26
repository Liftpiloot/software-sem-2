namespace Iron_Domain;

public class Set
{
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    public void AddSet(int executionId)
    {
        SqlConnection conn = new SqlConnection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercise_sets (ExerciseExecutionID, SetWeight, SetRepetitions) VALUES (@executionid,@weight, @reps)", conn);
        cmd.Parameters.AddWithValue("@executionid", executionId);
        cmd.Parameters.AddWithValue("@weight", Weight);
        cmd.Parameters.AddWithValue("@reps", Reps);
        cmd.ExecuteNonQuery();
    }
}