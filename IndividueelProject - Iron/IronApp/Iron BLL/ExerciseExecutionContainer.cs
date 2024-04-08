using Iron_DAL;
using Iron_Domain;
using Iron_DAL.DTO;

namespace IronDomain;

public class ExerciseExecutionContainer
{
    private readonly DbExerciseExecution _dbExerciseExecution = new();
    
    public int AddExerciseExecution(ExerciseExecution exerciseExecution)
    {
        ExerciseExecutionDto exerciseExecutionDto = new()
        {
            ExerciseId = exerciseExecution.ExerciseId,
            UserId = exerciseExecution.UserId,
            ExecutionDate = exerciseExecution.ExecutionDate,
        };
        int? returnedExerciseExecution = _dbExerciseExecution.AddExerciseExecution(exerciseExecutionDto);
        return returnedExerciseExecution ?? -1;
    }

    public bool AddSet(ExerciseExecution execution, Set set)
    {
        SetDto setDto = new()
        {
            ExerciseExecutionId = execution.Id,
            Reps = set.Reps,
            Weight = set.Weight,
        };
        
        return _dbExerciseExecution.AddSet(setDto);
        
    }

    public ExerciseExecution? GetRecentExerciseExecution(User user, Exercise exercise)
    {
        UserDto userDto = new()
        {
            Id = user.Id,
        };
        ExerciseDto exerciseDto = new()
        {
            Id = exercise.Id,
        };
        ExerciseExecutionDto? exerciseExecutionDto = _dbExerciseExecution.GetRecentExecution(userDto, exerciseDto);
        if (exerciseExecutionDto == null)
        {
            return null;
        }
        ExerciseExecution? exerciseExecution = new()
        {
            Id = exerciseExecutionDto.Id,
            ExecutionDate = exerciseExecutionDto.ExecutionDate,
            UserId = exerciseExecutionDto.UserId,
            ExerciseId = exerciseExecutionDto.ExerciseId,
        };
        return exerciseExecution;
    }

    public List<Set> GetSets(ExerciseExecution? execution)
    {
        ExerciseExecutionDto exerciseExecutionDto = new()
        {
            Id = execution?.Id,
        };
        List<SetDto> setDtos = _dbExerciseExecution.GetSets(exerciseExecutionDto);
        List<Set> sets = new();
        foreach (SetDto setDto in setDtos)
        {
            sets.Add(new Set()
            {
                Reps = setDto.Reps,
                Weight = setDto.Weight,
            });
        }
        return sets;
    }

    public List<ExerciseExecution> GetExerciseExecutions(User user, Exercise exercise)
    {
        UserDto userDto = new()
        {
            Id = user.Id,
        };
        ExerciseDto exerciseDto = new()
        {
            Id = exercise.Id,
        };
        List<ExerciseExecutionDto> exerciseExecutionDtos = _dbExerciseExecution.GetExerciseExecutions(userDto, exerciseDto);
        List<ExerciseExecution> exerciseExecutions = new();
        foreach (ExerciseExecutionDto exerciseExecutionDto in exerciseExecutionDtos)
        {
            exerciseExecutions.Add(new ExerciseExecution()
            {
                Id = exerciseExecutionDto.Id,
                ExecutionDate = exerciseExecutionDto.ExecutionDate,
                UserId = exerciseExecutionDto.UserId,
                ExerciseId = exerciseExecutionDto.ExerciseId,
            });
        }
        return exerciseExecutions;
    }
}