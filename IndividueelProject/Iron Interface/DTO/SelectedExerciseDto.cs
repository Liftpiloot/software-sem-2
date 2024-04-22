namespace Iron_DAL.DTO;

public record SelectedExerciseDto()
{
    public int UserId { get; init; }
    public int ExerciseId { get; init; }
}