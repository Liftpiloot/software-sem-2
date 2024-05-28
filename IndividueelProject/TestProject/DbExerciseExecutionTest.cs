using Iron_DAL.DTO;
using Iron_Interface;

namespace TestProject;

public class DbExerciseExecutionTest : IDbExerciseExecution
{
    public int AddExerciseExecution(ExerciseExecutionDto exercise)
    {
        if (exercise.ExerciseId != 0)
        {
            if (exercise.UserId != 0)
            {
                return 1;
            }
        }
        return -1;
    }

    public List<SetDto> GetSets(ExerciseExecutionDto exerciseExecution)
    {
        if (exerciseExecution.Id > 0)
        {
            return new List<SetDto>
            {
                new SetDto
                {
                    ExerciseExecutionId = exerciseExecution.Id,
                    Reps = 10,
                    Weight = 10
                },
                new SetDto
                {
                    ExerciseExecutionId = exerciseExecution.Id,
                    Reps = 10,
                    Weight = 10
                }
            };
        }
        return new List<SetDto>();
    }

    public bool AddSet(SetDto set)
    {
        if (set.ExerciseExecutionId != 0)
        {
            if (set.Reps != 0)
            {
                if (set.Weight != 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public ExerciseExecutionDto? GetRecentExecution(UserDto user, ExerciseDto exercise)
    {
        if (user.Id != 0)
        {
            if (exercise.Id != 0)
            {
                return new ExerciseExecutionDto
                {
                    Id = 1,
                    UserId = user.Id,
                    ExerciseId = exercise.Id,
                    ExecutionDate = DateTime.Now
                };
            }
        }
        return null;
    }

    public List<ExerciseExecutionDto> GetExerciseExecutions(UserDto user, ExerciseDto exercise)
    {
        if (user.Id != 0)
        {
            if (exercise.Id != 0)
            {
                return new List<ExerciseExecutionDto>
                {
                    new ExerciseExecutionDto
                    {
                        Id = 1,
                        UserId = user.Id,
                        ExerciseId = exercise.Id,
                        ExecutionDate = DateTime.Now
                    },
                    new ExerciseExecutionDto
                    {
                        Id = 2,
                        UserId = user.Id,
                        ExerciseId = exercise.Id,
                        ExecutionDate = DateTime.Now
                    }
                };
            }
        }
        return new List<ExerciseExecutionDto>();
    }

    public bool IsPersonalBest(ExerciseExecutionDto exerciseExecutionDto, List<SetDto> setDtos)
    {
        if (exerciseExecutionDto.Id != 0)
        {
            if (setDtos.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public List<ExerciseExecutionDto> GetAllExecutionsForUser(int userId)
    {
        throw new NotImplementedException();
    }
}