using Iron_DAL;
using Iron_DAL.DTO;
using Iron_Domain;
using Iron_Interface;

namespace IronDomain;

public class ExerciseContainer
{
    private readonly IDbExercise _dbExercise;
    
    public ExerciseContainer(IDbExercise db)
    {
        _dbExercise = db;
    }
    public int AddExercise(Exercise exercise)
    {
        ExerciseDto exerciseDto = new()
        {
            Name = exercise.Name,
            UserId = exercise.UserId,
            Description = exercise.Description,
            Logo = exercise.Logo
        };
        var returnedExercise = _dbExercise.AddExercise(exerciseDto);
        return returnedExercise;
    }
    
    public bool DeleteExercise(User user, Exercise exercise)
    {
        ExerciseDto exerciseDto = new()
        {
            Id = exercise.Id
        };
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

        Exercise newExercise = new()
        {
            Id = returnedExercise.Id,
            UserId = returnedExercise.UserId,
            Name = returnedExercise.Name,
            Description = returnedExercise.Description,
            Logo = returnedExercise.Logo
            
        };
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
                Exercise exercise = new()
                {
                    Id = exerciseDto.Id,
                    UserId = exerciseDto.UserId,
                    Name = exerciseDto.Name,
                    Description = exerciseDto.Description,
                    Logo = exerciseDto.Logo
                };
                exercises.Add(exercise);
            }
            return exercises;
        }
    
    public bool AddSelectedExercise(SelectedExercise selectedExercise)
    {
        SelectedExerciseDto selectedExerciseDto = new()
        {
            UserId = selectedExercise.UserId,
            ExerciseId = selectedExercise.ExerciseId
        };
        return _dbExercise.AddSelectedExercise(selectedExerciseDto);
    }
    
    public bool DeleteSelectedExercise(SelectedExercise selectedExercise)
    {
        SelectedExerciseDto selectedExerciseDto = new()
        {
            UserId = selectedExercise.UserId,
            ExerciseId = selectedExercise.ExerciseId
        };
        
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
            SelectedExercise selectedExercise = new()
            {
                UserId = selectedExerciseDto.UserId,
                ExerciseId = selectedExerciseDto.ExerciseId
            };
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
            Exercise exercise = new()
            {
                Id = exerciseDto.Id,
                UserId = exerciseDto.UserId,
                Name = exerciseDto.Name,
                Description = exerciseDto.Description,
                Logo = exerciseDto.Logo
            };
            exercises.Add(exercise);
        }
        return exercises;
    }
    

    
    
    
    
}