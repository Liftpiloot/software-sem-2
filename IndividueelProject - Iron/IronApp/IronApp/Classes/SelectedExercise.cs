using IronApp.Models;
using Microsoft.Data.SqlClient;

namespace IronApp.Classes;

public class SelectedExercise
{
    ExerciseType Type { get; set; }
    public int ExerciseTypeId { get; set; }
    
    public void AddSelectedExercise(User user)
    {
        SqlConnection conn = new SqlConnection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO SelectedExercise (UserID, ExerciseType, ExerciseTypeId) VALUES (@userid ,@type, @exercisetypeid)", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@type", Type);
        cmd.Parameters.AddWithValue("@exercisetypeid", ExerciseTypeId);
        cmd.ExecuteNonQuery();
        conn.Close();
    }
    
    public void RemoveSelectedExercise(User user)
    {
        SqlConnection conn = new SqlConnection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM SelectedExercise WHERE UserID = @userid AND ExerciseType = @type AND ExerciseTypeId = @exercisetypeid", conn);
        cmd.Parameters.AddWithValue("@userid", user.Id);
        cmd.Parameters.AddWithValue("@type", Type);
        cmd.Parameters.AddWithValue("@exercisetypeid", ExerciseTypeId);
        cmd.ExecuteNonQuery();
        conn.Close();
    }
}