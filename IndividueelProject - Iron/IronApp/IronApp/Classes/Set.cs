using Microsoft.Data.SqlClient;

namespace IronApp.Classes;

public class Set
{
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    public void AddSet()
    {
        SqlConnection conn = new SqlConnection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO sets (Weight, Reps) VALUES (@weight, @reps)", conn);
        cmd.Parameters.AddWithValue("@weight", Weight);
        cmd.Parameters.AddWithValue("@reps", Reps);
        cmd.ExecuteNonQuery();
    }
}