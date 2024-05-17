using Iron_Domain;
using Iron_Interface;
using IronDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject;

[TestClass]
public class UnitTestExerciseExecution
{
    private static readonly IDbExerciseExecution Db = new DbExerciseExecutionTest();
    private readonly ExerciseExecutionContainer _exerciseExecutionContainer = new(Db);
    
    [TestMethod]
    public void AddExerciseExecution()
    {
        ExerciseExecution exerciseExecution = new()
        {
            ExerciseId = 1,
            UserId = 1
        };
        int id = _exerciseExecutionContainer.AddExerciseExecution(exerciseExecution);
        Assert.AreEqual(1, id);
    }
    
    [TestMethod]
    public void GetSets()
    {
        ExerciseExecution exerciseExecution = new()
        {
            Id = 1
        };
        List<Set> sets = _exerciseExecutionContainer.GetSets(exerciseExecution);
        Assert.IsNotNull(sets);
    }
    
    [TestMethod]
    public void AddSet()
    {
        Set set = new()
        {
            Reps = 10,
            Weight = 10
        };
        ExerciseExecution execution = new()
        {
            Id = 1
        };
        bool added = _exerciseExecutionContainer.AddSet(execution, set);
        Assert.IsTrue(added);
    }
    
    [TestMethod]
    public void GetRecentExecution()
    {
        User user = new()
        {
            Id = 1
        };
        Exercise exercise = new()
        {
            Id = 1
        };
        ExerciseExecution? exerciseExecution = _exerciseExecutionContainer.GetRecentExerciseExecution(user, exercise);
        Assert.IsNotNull(exerciseExecution);
    }
    
    [TestMethod]
    public void GetExerciseExecutions()
    {
        User user = new()
        {
            Id = 1
        };
        Exercise exercise = new()
        {
            Id = 1
        };
        List<ExerciseExecution> exerciseExecutions = _exerciseExecutionContainer.GetExerciseExecutions(user, exercise);
        Assert.IsNotNull(exerciseExecutions);
    }
    
    [TestMethod]
    public void IsPersonalBest()
    {
        ExerciseExecution exerciseExecution = new()
        {
            Id = 1
        };
        List<Set> sets = new()
        {
            new Set
            {
                Reps = 10,
                Weight = 10
            }
        };
        bool isPersonalBest = _exerciseExecutionContainer.IsPersonalBest(exerciseExecution, sets);
        Assert.IsTrue(isPersonalBest);
    }
}