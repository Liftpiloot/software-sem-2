using IronApp.Models;
using Microsoft.Data.SqlClient;

namespace IronApp.Classes
{
    public class Exercise
    {
        private string db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";
        public void GetExercises(int userId)
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM exercises WHERE UserId = @id", conn);
            cmd.Parameters.AddWithValue("@id", userId);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["name"].ToString());
            }

        }

        public List<ExerciseModel> GetRecentExercises(int userId)
        {
            // get all exercises with unique names, and only show the most recent ones
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                $"SELECT Name, max(Date), ExerciseTypeID, ExerciseType " +
                $"FROM Exercises WHERE UserId={userId} " +
                $"GROUP BY Name, ExerciseTypeID, ExerciseType", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<ExerciseModel> exercises = new List<ExerciseModel>();
            while (reader.Read())
            {
                exercises.Add(new ExerciseModel
                {
                    Name = reader["Name"].ToString(),
                    ExerciseTypeId = Convert.ToInt32(reader["ExerciseTypeId"]),
                    type = (ExerciseType)Enum.Parse(typeof(ExerciseType), reader["ExerciseType"].ToString())
                });
            }
            reader.Close();
            return AddExerciseInfo(exercises);
        }

        public List<ExerciseModel> AddExerciseInfo(List<ExerciseModel> exercises)
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            foreach (var exercise in exercises)
            {
                int id = exercise.ExerciseTypeId;
                if (exercise.type == ExerciseType.custom)
                {
                    SqlCommand cmd = new SqlCommand($"SELECT * from custom_exercises WHERE CustomExerciseID = {id}", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        exercise.Description = reader["Description"].ToString();
                        exercise.logo = reader["logo"].ToString();
                    }
                    reader.Close();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand($"SELECT * from predefined_exercises WHERE PredefinedExerciseID = {id}", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        exercise.Description = reader["Description"].ToString();
                        exercise.logo = reader["logo"].ToString();
                    }
                    reader.Close();
                }
            }
            return exercises;
        }
    }


}
