using Iron_DAL;
using Iron_Domain;
using Iron_DAL.DTO;
using Iron_Interface;

namespace IronDomain;

public class ExerciseExecutionContainer
{
    private readonly IDbExerciseExecution _dbExerciseExecution;
    
    public ExerciseExecutionContainer(IDbExerciseExecution db)
    {
        _dbExerciseExecution = db;
    }

    public int AddExerciseExecution(ExerciseExecution exerciseExecution)
    {
        ExerciseExecutionDto exerciseExecutionDto = new()
        {
            ExerciseId = exerciseExecution.ExerciseId,
            UserId = exerciseExecution.UserId,
            ExecutionDate = exerciseExecution.ExecutionDate,
        };
        var returnedExerciseExecution = _dbExerciseExecution.AddExerciseExecution(exerciseExecutionDto);
        return returnedExerciseExecution;
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
        List<ExerciseExecutionDto> exerciseExecutionDtos =
            _dbExerciseExecution.GetExerciseExecutions(userDto, exerciseDto);
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

    public bool AddWorkout(List<ExerciseExecution> exerciseExecutions)
    {
        foreach (ExerciseExecution exerciseExecution in exerciseExecutions)
        {
            ExerciseExecutionDto exerciseExecutionDto = new()
            {
                ExerciseId = exerciseExecution.ExerciseId,
                UserId = exerciseExecution.UserId,
                ExecutionDate = exerciseExecution.ExecutionDate,
            };
            var returnedExerciseExecution = _dbExerciseExecution.AddExerciseExecution(exerciseExecutionDto);
            if (returnedExerciseExecution <= 0)
            {
                return false;
            }
            foreach (var setDto in exerciseExecution.Sets.Select(set => new SetDto()
                     {
                         ExerciseExecutionId = returnedExerciseExecution,
                         Reps = set.Reps,
                         Weight = set.Weight,
                     }))
            {
                if (!_dbExerciseExecution.AddSet(setDto))
                {
                    return false;
                }
            }
        }

        return true;
    }
}