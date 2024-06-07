namespace IronDomain;

public class SelectedExercise
{
    public int UserId { get;}
    public int ExerciseId { get;}
    
    public SelectedExercise(int userId, int exerciseId)
    {
        UserId = userId;
        ExerciseId = exerciseId;
    }
}