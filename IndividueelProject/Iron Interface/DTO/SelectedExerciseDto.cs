namespace Iron_Interface.DTO;

public record SelectedExerciseDto
{
    public int UserId { get; init; }
    public int ExerciseId { get; init; }
}