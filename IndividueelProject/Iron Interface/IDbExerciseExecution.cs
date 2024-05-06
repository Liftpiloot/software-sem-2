using Iron_DAL.DTO;

namespace Iron_Interface;

public interface IDbExerciseExecution
{
    /// <summary>
    /// Add an exercise execution to the database
    /// </summary>
    /// <param name="exercise"></param>
    /// <returns>Execution id, -1 if adding failed</returns>
    public int AddExerciseExecution(ExerciseExecutionDto exercise);
    
    /// <summary>
    /// Gets the sets of the exercise execution.
    /// </summary>
    /// <param name="exerciseExecution"></param>
    /// <returns>List of set</returns>
    public List<SetDto> GetSets(ExerciseExecutionDto exerciseExecution);
    
    /// <summary>
    /// Adds set to exercise execution
    /// </summary>
    /// <param name="set"></param>
    /// <returns>true if successful</returns>
    public bool AddSet(SetDto set);
    
    /// <summary>
    /// Gets the most recent exercise execution of a user and exercise
    /// </summary>
    /// <param name="user"></param>
    /// <param name="exercise"></param>
    /// <returns>Exercise Execution, null if not found</returns>
    public ExerciseExecutionDto? GetRecentExecution(UserDto user, ExerciseDto exercise);
    
    /// <summary>
    /// Gets the most recent exercise executions of unique names of a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of exercise executions</returns>
    public List<ExerciseExecutionDto> GetRecentExerciseExecutions(UserDto user);
    
    /// <summary>
    /// get all executions of a user of one exercise
    /// </summary>
    /// <param name="user"></param>
    /// <param name="exercise"></param>
    /// <returns>List of exercise executions</returns>
    public List<ExerciseExecutionDto> GetExerciseExecutions(UserDto user, ExerciseDto exercise);

    public bool IsPersonalBest(ExerciseExecutionDto exerciseExecutionDto, List<SetDto> setDtos);
}