
namespace Iron_Domain
{
    public class ExerciseExecution
    {
        public int? Id { get; set; }
        public DateTime ExecutionDate { get; set; }
        public int? UserId { get; set; }
        public int ExerciseId { get; set; }
        public List<Set> Sets { get; set; } = new();
    }
}