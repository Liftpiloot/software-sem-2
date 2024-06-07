using System.Data;
using Iron_Domain;
using Iron_Interface;
using IronDomain;
using IronDomain.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject;

[TestClass]
public class UnitTestExerciseExecution
{
    private static IDbExerciseExecution _mockDbExerciseExecution;
    private ExerciseExecutionContainer _container;

    [TestInitialize]
    public void Setup()
    {
        _mockDbExerciseExecution = new DbExerciseExecutionTest();
        _container = new ExerciseExecutionContainer(_mockDbExerciseExecution);
    }

    [TestMethod]
    public void AddExerciseExecution_ShouldReturnId()
    {
        // Arrange
        var exerciseExecution = new ExerciseExecution(1, DateTime.Now, 1, 1, new List<Set>());

        // Act
        var result = _container.AddExerciseExecution(exerciseExecution);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void AddSet_ShouldReturnTrue()
    {
        // Arrange
        var set = new Set(100, 10) { ExerciseExecutionId = 1 };
        var exerciseExecution = new ExerciseExecution(1, DateTime.Now, 1, 1, new List<Set> { set });

        // Act
        var (result, errors) = _container.AddSet(exerciseExecution, set);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetRecentExerciseExecution_ShouldReturnExecution()
    {
        // Arrange
        var user = new User(1, "Test", "Test", "Test", DateTime.Now, 1);
        var exercise = new Exercise(1, 1, "Test", "Test", "/images/default.png");

        // Act
        var result = _container.GetRecentExerciseExecution(user, exercise);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [TestMethod]
    public void GetSets_ShouldReturnSets()
    {
        // Arrange
        var execution = new ExerciseExecution(1, DateTime.Now, 1, 1, new List<Set>());

        // Act
        var result = _container.GetSets(execution);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(10, result[0].Reps);
        Assert.AreEqual(10, result[0].Weight);
    }

    [TestMethod]
    public void GetExerciseExecutions_ShouldReturnExecutions()
    {
        // Arrange
        var user = new User(1, "Test", "Test", "Test", DateTime.Now, 1);
        var exercise = new Exercise(1, 1, "Test", "Test", "/images/default.png");

        // Act
        var result = _container.GetExerciseExecutions(user, exercise);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(1, result[0].Id);
        Assert.AreEqual(2, result[1].Id);
    }

    [TestMethod]
    public void AddWorkout_ShouldReturnTrue()
    {
        // Arrange
        var set = new Set(100, 10);
        var exerciseExecution = new ExerciseExecution(1, DateTime.Now, 1, 1, new List<Set> { set });
        var exerciseExecutions = new List<ExerciseExecution> { exerciseExecution };

        // Act
        var result = _container.AddWorkout(exerciseExecutions);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsPersonalBest_ShouldReturnTrue()
    {
        // Arrange
        var set = new Set(100, 10);
        var exerciseExecution = new ExerciseExecution(1, DateTime.Now, 1, 1, new List<Set> { set });
        var sets = new List<Set> { set };

        // Act
        var result = _container.IsPersonalBest(exerciseExecution, sets);

        // Assert
        Assert.IsTrue(result);
    }
    

}