namespace Iron_DAL.DTO;

public record ExerciseExecutionDto
{
    public int Id { get; init; }
    public DateTime ExecutionDate { get; init; }
    public int UserId { get; init; }
    public int ExerciseId { get; init; }
}