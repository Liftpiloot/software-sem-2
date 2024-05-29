
namespace Iron_Domain
{
    public class ExerciseExecution
    {
        public int Id { get;}
        public DateTime ExecutionDate { get;}
        public int UserId { get;}
        public int ExerciseId { get;}
        public List<Set> Sets { get; set; }
        
        public ExerciseExecution(int id, DateTime executionDate, int userId, int exerciseId, List<Set> sets)
        {
            Id = id;
            ExecutionDate = executionDate;
            UserId = userId;
            ExerciseId = exerciseId;
            Sets = sets;
        }
    }
}