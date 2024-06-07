namespace Iron_Interface.DTO;

public record ExerciseExecutionDto
{
    public int Id { get; init; }
    public DateTime ExecutionDate { get; init; }
    public int UserId { get; init; }
    public int ExerciseId { get; init; }
    public List<SetDto> Sets { get; init; }
}