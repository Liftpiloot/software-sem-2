using Iron_Interface;
using Iron_Interface.DTO;

namespace TestProject;

public class DbExerciseTest : IDbExercise
{
    public int AddExercise(ExerciseDto exercise)
    {
        return 1;
    }

    public ExerciseDto? GetExercise(ExerciseDto exercise)
    {
        if (exercise.Id != 0)
        {
            return new ExerciseDto
            {
                Id = exercise.Id,
                UserId = 1,
                Name = "Test",
                Description = "Test",
                Logo = "/images/default.png"
            };
        }
        return null;
    }

    public bool AddSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        if (selectedExerciseDto.ExerciseId != 0)
        {
            if (selectedExerciseDto.UserId != 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool DeleteSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        if (selectedExerciseDto.ExerciseId != 0)
        {
            if (selectedExerciseDto.UserId != 0)
            {
                return true;
            }
        }
        return false;
    }

    public List<SelectedExerciseDto> GetSelectedExercises(UserDto user)
    {
        List<SelectedExerciseDto> selectedExercises = new()
        {
            new SelectedExerciseDto
            {
                UserId = user.Id,
                ExerciseId = 1
            },
            new SelectedExerciseDto
            {
                UserId = user.Id,
                ExerciseId = 2
            }
        };
        return selectedExercises;
    }

    public List<ExerciseDto> GetUnselectedExercises(UserDto user)
    {
        List<ExerciseDto> exercises = new()
        {
            new ExerciseDto
            {
                Id = 1,
                UserId = user.Id,
                Name = "Test",
                Description = "Test",
                Logo = "/images/default.png"
            },
            new ExerciseDto
            {
                Id = 2,
                UserId = user.Id,
                Name = "Test",
                Description = "Test",
                Logo = "/images/default.png"
            }
        };
        return exercises;
    }

    public List<ExerciseDto> GetExercises(UserDto user)
    {
        List<ExerciseDto> exercises = new()
        {
            new ExerciseDto
            {
                Id = 1,
                UserId = user.Id,
                Name = "Test",
                Description = "Test",
                Logo = "/images/default.png"
            },
        };
        return exercises;
    }

    public bool DeleteExercise(UserDto userDto, ExerciseDto exerciseDto)
    {
        if (exerciseDto.Id != 0)
        {
            if (userDto.Id != 0)
            {
                return true;
            }
        }
        return false;
    }
}