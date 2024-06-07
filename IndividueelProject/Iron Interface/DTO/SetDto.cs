namespace Iron_Interface.DTO;

public record SetDto
{
    public decimal Weight { get; init; }
    public int Reps { get; init; }
    public int? ExerciseExecutionId { get; init; }
}