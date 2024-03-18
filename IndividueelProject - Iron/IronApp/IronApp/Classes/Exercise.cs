﻿using Microsoft.Data.SqlClient;

namespace IronApp.Classes;

public class Exercise
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    
    private readonly string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
    
    public void AddExercise()
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("INSERT INTO exercises (UserID, ExerciseName, ExerciseDescription, LogoFilePath) VALUES (@userid, @name, @description, @logo)", conn);
        cmd.Parameters.AddWithValue("@userid", UserId);
        cmd.Parameters.AddWithValue("@name", Name);
        cmd.Parameters.AddWithValue("@description", Description);
        cmd.Parameters.AddWithValue("@logo", Logo);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public ExerciseExecution? GetRecentExecution(int userId)
    {
        SqlConnection conn = new SqlConnection(_db);
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM exercise_executions WHERE ExerciseID = @id AND UserID = @userid ORDER BY ExerciseExecutionDate DESC", conn);
        cmd.Parameters.AddWithValue("@id", Id);
        cmd.Parameters.AddWithValue("@userid", userId);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            ExerciseExecution exerciseExecution = new ExerciseExecution
            {
                Id = (int)reader["ExerciseExecutionID"],
                ExerciseId = (int)reader["ExerciseID"]
            };
            return exerciseExecution;
        }
        return null;
    }
    
}