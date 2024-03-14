using IronApp.Models;
using Microsoft.Data.SqlClient;

namespace IronApp.Classes
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExerciseTypeId { get; set; }
        public ExerciseType Type { get; set; }

        private string db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";

        public void GetExercises(int userId)
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Exercises WHERE UserId = @id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["name"].ToString());
            }
        }

        public void AddExercise()
        {
            // TODO add exercise
        }


        public dynamic GetExerciseInfo()
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();

            if (Type == ExerciseType.Custom)
            {
                SqlCommand cmd = new SqlCommand($"SELECT * from custom_exercises WHERE CustomExerciseID = @exerciseId",
                    conn);
                cmd.Parameters.AddWithValue("@exerciseId", ExerciseTypeId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    CustomExercise exercise = new CustomExercise();
                    exercise.Id = (int)reader["CustomExerciseID"];
                    exercise.Name = reader["Name"].ToString();
                    exercise.Description = reader["Description"].ToString();
                    reader.Close();
                    return exercise;
                }
            }

            else
            {
                SqlCommand cmd =
                    new SqlCommand($"SELECT * from predefined_exercises WHERE PredefinedExerciseID = @exerciseId",
                        conn);
                cmd.Parameters.AddWithValue("@exerciseId", ExerciseTypeId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    PreDefinedExercise exercise = new PreDefinedExercise();
                    exercise.Id = (int)reader["PredefinedExerciseID"];
                    exercise.Name = reader["Name"].ToString();
                    exercise.Description = reader["Description"].ToString();
                    exercise.Logo = reader["logo"].ToString();
                    reader.Close();
                    return exercise;
                }
            }

            conn.Close();
            CustomExercise customExercise = new CustomExercise();
            customExercise.Name = "No exercise found";
            return customExercise;
        }

        public List<Set> GetSets()
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            List<Set> sets = new List<Set>();
            SqlCommand cmd = new SqlCommand($"SELECT * from sets WHERE ExerciseID = @id", conn);
            cmd.Parameters.AddWithValue("@id", Id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sets.Add(new Set()
                {
                    Weight = (Decimal)reader["Weight"],
                    Reps = (int)reader["Reps"]
                });
            }
            
            reader.Close();
            conn.Close();
            return sets;
        }
    }
}