namespace IronApp.Models
{
    public class ExerciseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string logo { get; set; }
        public bool selected { get; set; }
        public ExerciseType type { get; set; }

        public int ExerciseTypeId { get; set; }

    }
    public enum ExerciseType
    {
        predefined,
        custom
    }
}
