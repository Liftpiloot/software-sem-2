using System.Runtime.InteropServices.JavaScript;

namespace Iron_Domain
{
    public class ExerciseExecution
    {
        public int? Id { get; set; }
        public JSType.Date? ExecutionDate { get; set; }
        public int? UserId { get; set; }
        public int ExerciseId { get; set; }
    }
}