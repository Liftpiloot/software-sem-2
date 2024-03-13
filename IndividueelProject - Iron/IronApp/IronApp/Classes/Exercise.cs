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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Exercises WHERE UserId = @id", conn);
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
                $"SELECT Name, max(Date), ExerciseTypeID, ExerciseType, ExerciseID " +
                $"FROM Exercises WHERE UserId={userId} " +
                $"GROUP BY Name, ExerciseTypeID, ExerciseType, ExerciseID", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<ExerciseModel> exercises = new List<ExerciseModel>();
            while (reader.Read())
            {
                exercises.Add(new ExerciseModel
                {
                    Id = (int)reader["ExerciseID"],
                    Name = reader["Name"].ToString(),
                    ExerciseTypeId = Convert.ToInt32(reader["ExerciseTypeId"]),
                    Type = (ExerciseType)Enum.Parse(typeof(ExerciseType), reader["ExerciseType"].ToString())
                });
            }
            conn.Close();
            reader.Close();
            exercises = AddExerciseInfo(exercises);
            exercises = addSets(exercises);
            return exercises;
        }

        public List<ExerciseModel> AddExerciseInfo(List<ExerciseModel> exercises)
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            foreach (var exercise in exercises)
            {
                int id = exercise.ExerciseTypeId;
                if (exercise.Type == ExerciseType.Custom)
                {
                    SqlCommand cmd = new SqlCommand($"SELECT * from custom_exercises WHERE CustomExerciseID = {id}", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        exercise.Description = reader["Description"].ToString();
                        exercise.Logo = reader["logo"].ToString();
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
                        exercise.Logo = reader["logo"].ToString();
                    }
                    reader.Close();
                }
            }

            conn.Close();
            return exercises;
        }

        public List<ExerciseModel> addSets(List<ExerciseModel> exercises)
        {
            SqlConnection conn = new SqlConnection(db);
            conn.Open();
            foreach (var exercise in exercises)
            {
                List<Set> sets = new List<Set>();
                int id = exercise.Id;
                SqlCommand cmd = new SqlCommand($"SELECT * from sets WHERE ExerciseID = {id}", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sets.Add(new Set()
                    {
                        Weight = (Decimal)reader["Weight"],
                        Reps = (int)reader["Reps"]
                    });
                }
                exercise.Sets = sets;
                reader.Close();
            }

            conn.Close();
            return exercises;
        }
    }


}
