using Iron_Domain;
using Iron_Interface;
using Iron_Interface.DTO;

namespace IronDomain.Containers;

public class ExerciseContainer
{
    private readonly IDbExercise _dbExercise;

    public ExerciseContainer(IDbExercise db)
    {
        _dbExercise = db;
    }

    // method to convert exercise dto to exercise
    private static Exercise ConvertToExercise(ExerciseDto exerciseDto)
    {
        Exercise exercise = new Exercise(exerciseDto.Id, exerciseDto.UserId, exerciseDto.Name,
            exerciseDto.Description, exerciseDto.Logo);
        return exercise;
    }

    // method to convert exercise to exercise dto
    private static ExerciseDto ConvertToExerciseDto(Exercise exercise)
    {
        ExerciseDto exerciseDto = new()
        {
            Id = exercise.Id,
            UserId = exercise.UserId,
            Name = exercise.Name,
            Description = exercise.Description,
            Logo = exercise.Logo
        };
        return exerciseDto;
    }
    
    // method to convert selected exercise dto to selected exercise
    private static SelectedExercise ConvertToSelectedExercise(SelectedExerciseDto selectedExerciseDto)
    {
        SelectedExercise selectedExercise = new SelectedExercise(selectedExerciseDto.UserId, selectedExerciseDto.ExerciseId);
        return selectedExercise;
    }
    
    // method to convert selected exercise to selected exercise dto
    private static SelectedExerciseDto ConvertToSelectedExerciseDto(SelectedExercise selectedExercise)
    {
        SelectedExerciseDto selectedExerciseDto = new()
        {
            UserId = selectedExercise.UserId,
            ExerciseId = selectedExercise.ExerciseId
        };
        return selectedExerciseDto;
    }


    public int AddExercise(Exercise exercise)
    {
        ExerciseDto exerciseDto = ConvertToExerciseDto(exercise);
        var returnedExercise = _dbExercise.AddExercise(exerciseDto);
        return returnedExercise;
    }

    public bool DeleteExercise(User user, Exercise exercise)
    {
        ExerciseDto exerciseDto = ConvertToExerciseDto(exercise);
        UserDto userDto = new()
        {
            Id = user.Id
        };
        return _dbExercise.DeleteExercise(userDto, exerciseDto);
    }

    public Exercise? GetExerciseFromId(int exerciseId)
    {
        ExerciseDto exerciseDto = new()
        {
            Id = exerciseId
        };
        ExerciseDto? returnedExercise = _dbExercise.GetExercise(exerciseDto);
        if (returnedExercise == null)
        {
            return null;
        }
        
        Exercise newExercise = ConvertToExercise(returnedExercise);
        return newExercise;
    }

    public List<Exercise> GetExercises(User user)
    {
        UserDto userDto = new()
        {
            Id = user.Id
        };
        List<ExerciseDto> exerciseDtos = _dbExercise.GetExercises(userDto);
        List<Exercise> exercises = new();
        foreach (ExerciseDto exerciseDto in exerciseDtos)
        {
            Exercise exercise = ConvertToExercise(exerciseDto);
            exercises.Add(exercise);
        }

        return exercises;
    }

    public bool AddSelectedExercise(SelectedExercise selectedExercise)
    {
        SelectedExerciseDto selectedExerciseDto = ConvertToSelectedExerciseDto(selectedExercise);
        return _dbExercise.AddSelectedExercise(selectedExerciseDto);
    }

    public bool DeleteSelectedExercise(SelectedExercise selectedExercise)
    {
        SelectedExerciseDto selectedExerciseDto = ConvertToSelectedExerciseDto(selectedExercise);

        return _dbExercise.DeleteSelectedExercise(selectedExerciseDto);
    }

    public List<SelectedExercise> GetSelectedExercises(User user)
    {
        UserDto userDto = new()
        {
            Id = user.Id
        };
        List<SelectedExerciseDto> selectedExerciseDtos = _dbExercise.GetSelectedExercises(userDto);
        List<SelectedExercise> selectedExercises = new();
        foreach (SelectedExerciseDto selectedExerciseDto in selectedExerciseDtos)
        {
            SelectedExercise selectedExercise = ConvertToSelectedExercise(selectedExerciseDto);
            selectedExercises.Add(selectedExercise);
        }

        return selectedExercises;
    }

    public List<Exercise> GetUnselectedExercises(User user)
    {
        UserDto userDto = new()
        {
            Id = user.Id
        };
        List<ExerciseDto> exerciseDtos = _dbExercise.GetUnselectedExercises(userDto);
        List<Exercise> exercises = new();
        foreach (ExerciseDto exerciseDto in exerciseDtos)
        {
            Exercise exercise = ConvertToExercise(exerciseDto);
            exercises.Add(exercise);
        }
        return exercises;
    }
}