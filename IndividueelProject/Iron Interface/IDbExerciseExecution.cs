using Iron_Interface.DTO;

namespace Iron_Interface;

public interface IDbExerciseExecution
{
    /// <summary>
    /// Add an exercise execution to the database
    /// </summary>
    /// <param name="exercise"></param>
    /// <returns>Execution id, -1 if adding failed</returns>
    int AddExerciseExecution(ExerciseExecutionDto exercise);
    
    /// <summary>
    /// Gets the sets of the exercise execution.
    /// </summary>
    /// <param name="exerciseExecution"></param>
    /// <returns>List of set</returns>
    List<SetDto> GetSets(ExerciseExecutionDto exerciseExecution);
    
    /// <summary>
    /// Adds set to exercise execution
    /// </summary>
    /// <param name="set"></param>
    /// <returns>true if successful</returns>
    bool AddSet(SetDto set);
    
    /// <summary>
    /// Gets the most recent exercise execution of a user and exercise
    /// </summary>
    /// <param name="user"></param>
    /// <param name="exercise"></param>
    /// <returns>Exercise Execution, null if not found</returns>
    ExerciseExecutionDto? GetRecentExecution(UserDto user, ExerciseDto exercise);

    /// <summary>
    /// get all executions of a user of one exercise
    /// </summary>
    /// <param name="user"></param>
    /// <param name="exercise"></param>
    /// <returns>List of exercise executions</returns>
    List<ExerciseExecutionDto> GetExerciseExecutions(UserDto user, ExerciseDto exercise);

    bool IsPersonalBest(ExerciseExecutionDto exerciseExecutionDto, List<SetDto> setDtos);
    List<ExerciseExecutionDto> GetAllExecutionsForUser(int userId);
}