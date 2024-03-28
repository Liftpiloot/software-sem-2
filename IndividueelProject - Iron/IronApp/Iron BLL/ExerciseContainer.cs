using Iron_DAL;
using Iron_DAL.DTO;
using Iron_Domain;

namespace IronDomain;

public class ExerciseContainer
{
    private readonly DbExercise _dbExercise = new();
    
    public int AddExercise(User user, Exercise exercise)
    {
        ExerciseDto exerciseDto = new()
        {
            Name = exercise.Name,
            Description = exercise.Description
        };
        UserDto userDto = new()
        {
            Id = user.Id
        };
        int? returnedExercise = _dbExercise.AddExercise(userDto, exerciseDto);
        return returnedExercise ?? -1;
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
        SelectedExerciseDTO selectedExerciseDto = new()
        {
            UserId = selectedExercise.UserId,
            ExerciseId = selectedExercise.ExerciseId
        };
        return _dbExercise.AddSelectedExercise(selectedExerciseDto);
    }
    
    public bool DeleteSelectedExercise(SelectedExercise selectedExercise)
    {
        SelectedExerciseDTO selectedExerciseDto = new()
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
        List<SelectedExerciseDTO> selectedExerciseDtos = _dbExercise.GetSelectedExercises(userDto);
        List<SelectedExercise> selectedExercises = new();
        foreach (SelectedExerciseDTO selectedExerciseDto in selectedExerciseDtos)
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

    
    
    
    
}