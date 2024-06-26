﻿namespace Iron_Interface.DTO;

public record ExerciseDto
{
    public int Id { get; init; }
    public int? UserId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Logo { get; init; }
}