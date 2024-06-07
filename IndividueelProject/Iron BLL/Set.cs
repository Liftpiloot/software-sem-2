namespace IronDomain;

public class Set
{
    public decimal Weight { get;}
    public int Reps { get;}
    public int ExerciseExecutionId { get; set; }

    public Set(decimal weight, int reps)
    {
        Weight = weight;
        Reps = reps;
    }
}