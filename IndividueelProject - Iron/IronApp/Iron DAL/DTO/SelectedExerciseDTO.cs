namespace Iron_DAL.DTO;

public record SelectedExerciseDTO()
{
    public int UserId { get; init; }
    public int ExerciseId { get; init; }
}