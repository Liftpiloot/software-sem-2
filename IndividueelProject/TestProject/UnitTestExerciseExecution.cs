using System.Data;
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
    [DataRow(1, 1)]
    [DataRow(0, 1)]
    [DataRow(1, 0)]
    [DataRow(0, 0)]
    public void AddExerciseExecution(int exerciseId, int userId)
    {
        ExerciseExecution exerciseExecution = new()
        {
            ExerciseId = exerciseId,
            UserId = userId
        };
        int id = _exerciseExecutionContainer.AddExerciseExecution(exerciseExecution);
        if (exerciseId == 0 || userId == 0)
        {
            Assert.AreEqual(-1, id);
        }
        else
        {
            Assert.AreEqual(1, id);
        }
    }
    
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    public void GetSets(int id)
    {
        ExerciseExecution exerciseExecution = new()
        {
            Id = id
        };
        List<Set> sets = _exerciseExecutionContainer.GetSets(exerciseExecution);
        Assert.AreEqual(id == 0 ? 0 : 2, sets.Count);
    }
    
    [TestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(1, 0, 0)]
    [DataRow(0, 1, 0)]
    [DataRow(0, 0, 1)]
    [DataRow(1, 10, 10)]
    public void AddSet(int id,int reps, int weight)
    {
        Set set = new()
        {
            Reps = reps,
            Weight = weight
        };
        ExerciseExecution execution = new()
        {
            Id = id
        };
        var added = _exerciseExecutionContainer.AddSet(execution, set);
        if (weight == 0 || reps==0 || id==0)
        {
            Assert.IsFalse(added);
        }
        else
        {
            Assert.IsTrue(added);
        }
    }
    
    [TestMethod]
    [DataRow(0, 0)]
    [DataRow(1, 0)]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    public void GetRecentExecution(int userId, int exerciseId)
    {
        User user = new()
        {
            Id = userId
        };
        Exercise exercise = new()
        {
            Id = exerciseId
        };
        ExerciseExecution? exerciseExecution = _exerciseExecutionContainer.GetRecentExerciseExecution(user, exercise);
        if (userId == 0 || exerciseId == 0)
        {
            Assert.IsNull(exerciseExecution);
        }
        else
        {
            Assert.IsNotNull(exerciseExecution);
        }
    }
    
    [TestMethod]
    [DataRow(0, 0)]
    [DataRow(1, 0)]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    public void GetExerciseExecutions(int userId, int exerciseId)
    {
        User user = new()
        {
            Id = userId
        };
        Exercise exercise = new()
        {
            Id = exerciseId
        };
        List<ExerciseExecution> exerciseExecutions = _exerciseExecutionContainer.GetExerciseExecutions(user, exercise);
        if (userId == 0 || exerciseId == 0)
        {
            Assert.AreEqual(0, exerciseExecutions.Count);
        }
        else
        {
            Assert.AreNotEqual(0, exerciseExecutions.Count);
        }
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