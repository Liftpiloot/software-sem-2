using System.Globalization;
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

    // method to convert exercise execution dto to exercise execution
    private ExerciseExecution ConvertToExerciseExecution(ExerciseExecutionDto exerciseExecutionDto)
    {
        List<Set> sets = new();
        foreach (SetDto setDto in exerciseExecutionDto.Sets)
        {
            sets.Add(ConvertToSet(setDto));
        }

        ExerciseExecution exerciseExecution = new ExerciseExecution(exerciseExecutionDto.Id,
            exerciseExecutionDto.ExecutionDate, exerciseExecutionDto.UserId, exerciseExecutionDto.ExerciseId, sets);
        return exerciseExecution;
    }

    // method to convert exercise execution to exercise execution dto
    private ExerciseExecutionDto ConvertToExerciseExecutionDto(ExerciseExecution exerciseExecution)
    {
        List<SetDto> setDtos = new();
        foreach (Set set in exerciseExecution.Sets)
        {
            setDtos.Add(ConvertToSetDto(set));
        }

        ExerciseExecutionDto exerciseExecutionDto = new()
        {
            Id = exerciseExecution.Id,
            UserId = exerciseExecution.UserId,
            ExerciseId = exerciseExecution.ExerciseId,
            ExecutionDate = exerciseExecution.ExecutionDate,
            Sets = setDtos
        };
        return exerciseExecutionDto;
    }

    // method to convert set dto to set
    private Set ConvertToSet(SetDto setDto)
    {
        Set set = new Set(setDto.Weight, setDto.Reps);
        return set;
    }

    // method to convert set to set dto
    private SetDto ConvertToSetDto(Set set)
    {
        SetDto setDto = new()
        {
            Reps = set.Reps,
            Weight = set.Weight,
        };
        return setDto;
    }

    public int AddExerciseExecution(ExerciseExecution exerciseExecution)
    {
        ExerciseExecutionDto exerciseExecutionDto = ConvertToExerciseExecutionDto(exerciseExecution);
        return _dbExerciseExecution.AddExerciseExecution(exerciseExecutionDto);
    }

    public bool AddSet(ExerciseExecution execution, Set set)
    {
        SetDto setDto = ConvertToSetDto(set);
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

        ExerciseExecution? exerciseExecution = ConvertToExerciseExecution(exerciseExecutionDto);
        return exerciseExecution;
    }

    public List<Set> GetSets(ExerciseExecution execution)
    {
        ExerciseExecutionDto exerciseExecutionDto = ConvertToExerciseExecutionDto(execution);
        List<SetDto> setDtos = _dbExerciseExecution.GetSets(exerciseExecutionDto);
        List<Set> sets = new();
        foreach (SetDto setDto in setDtos)
        {
            sets.Add(ConvertToSet(setDto));
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
            exerciseExecutions.Add(ConvertToExerciseExecution(exerciseExecutionDto));
        }

        return exerciseExecutions;
    }

    public bool AddWorkout(List<ExerciseExecution> exerciseExecutions)
    {
        foreach (ExerciseExecution exerciseExecution in exerciseExecutions)
        {
            ExerciseExecutionDto exerciseExecutionDto = ConvertToExerciseExecutionDto(exerciseExecution);
            var returnedExerciseExecution = _dbExerciseExecution.AddExerciseExecution(exerciseExecutionDto);
            if (returnedExerciseExecution <= 0)
            {
                return false;
            }

            if (exerciseExecution.Sets.Select(ConvertToSetDto).Any(setDto => !_dbExerciseExecution.AddSet(setDto)))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsPersonalBest(ExerciseExecution execution, List<Set> sets)
    {
        List<SetDto> setDtos = new();
        foreach (Set set in sets)
        {
            setDtos.Add(ConvertToSetDto(set));
        }
        return _dbExerciseExecution.IsPersonalBest(ConvertToExerciseExecutionDto(execution), setDtos);
    }

    public List<(int, int)> GetWorkoutsPerWeek(int userId)
    {
        // get all exercise executions for the user
        List<ExerciseExecutionDto> exerciseExecutions = _dbExerciseExecution.GetAllExecutionsForUser(userId);
        // group by week
        var workoutsPerWeek = exerciseExecutions
            .GroupBy(e =>
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.ExecutionDate, CalendarWeekRule.FirstDay,
                    DayOfWeek.Sunday))
            .Select(g => (Week: g.Key, DaysWithWorkout: g.Select(e => e.ExecutionDate.Date).Distinct().Count()))
            .ToList();
        return workoutsPerWeek;
    }
}