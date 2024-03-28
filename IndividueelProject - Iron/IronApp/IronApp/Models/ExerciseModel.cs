using Iron_Domain;

namespace IronApp.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public List<Set>? Sets { get; set; }
    }

    public enum ExerciseType
    {
        Predefined,
        Custom
    }
}
