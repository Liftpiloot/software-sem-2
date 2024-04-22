using Iron_DAL.DTO;

namespace Iron_Interface;

public interface IDbExercise
{
    /// <summary>
    /// Adds a custom exercise type to the database
    /// </summary>
    /// <param name="exercise"></param>
    /// <returns>Exercise Id, -1 if adding failed</returns>
    public int AddExercise(ExerciseDto exercise);
    
    /// <summary>
    /// Retrieve exercise information from the database, given the exercise id.
    /// </summary>
    /// <param name="exercise"></param>
    /// <returns>Exercise, null if not found</returns>
    public ExerciseDto? GetExercise(ExerciseDto exercise);
    
    /// <summary>
    /// Selects an exercise for a user, adding it to their list of exercises.
    /// </summary>
    /// <param name="selectedExerciseDto"></param>
    /// <returns>True if successful</returns>
    public bool AddSelectedExercise(SelectedExerciseDto selectedExerciseDto);
    
    /// <summary>
    /// Unselects an exercise for a user, removing it from their list of exercises.
    /// </summary>
    /// <param name="selectedExerciseDto"></param>
    /// <returns>True if successful</returns>
    public bool DeleteSelectedExercise(SelectedExerciseDto selectedExerciseDto);
    
    /// <summary>
    /// Gets all selected exercises of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of selected exercises</returns>
    public List<SelectedExerciseDto> GetSelectedExercises(UserDto user);
    
    /// <summary>
    /// Gets all exercises that are not selected by the user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of Exercises</returns>
    public List<ExerciseDto> GetUnselectedExercises(UserDto user);
    
    /// <summary>
    /// Gets all global exercises, and custom exercises of the user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of exercises</returns>
    public List<ExerciseDto> GetExercises(UserDto user);
    
    /// <summary>
    /// Deletes a custom exercise type from the database
    /// </summary>
    /// <param name="userDto"></param>
    /// <param name="exerciseDto"></param>
    /// <returns>True if successful</returns>
    public bool DeleteExercise(UserDto userDto, ExerciseDto exerciseDto);
    








}