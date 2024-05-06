using Iron_DAL.DTO;
using Iron_Domain;
using Iron_Interface;
using IronDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject;

[TestClass]
public class UnitTestExercise
{
    static IDbExercise db = new DbExerciseTest();
    ExerciseContainer exerciseContainer = new(db);
    
    [TestMethod]
    public void AddExercise()
    {
        Exercise exercise = new()
        {
            Name = "Test",
            UserId = 1,
            Description = "Test",
            Logo = "/images/default.png"
        };
        int id = exerciseContainer.AddExercise(exercise);
        Assert.AreEqual(1, id);
    }
    
    [TestMethod]
    public void DeleteExercise()
    {
        User user = new()
        {
            Id = 1
        };
        Exercise exercise = new()
        {
            Id = 1
        };
        bool deleted = exerciseContainer.DeleteExercise(user, exercise);
        Assert.IsTrue(deleted);
    }
    
    [TestMethod]
    public void GetExerciseFromId()
    {
        Exercise? exercise = exerciseContainer.GetExerciseFromId(1);
        Assert.IsNotNull(exercise);
    }
    
    [TestMethod]
    public void GetExercises()
    {
        User user = new()
        {
            Id = 1
        };
        List<Exercise> exercises = exerciseContainer.GetExercises(user);
        Assert.IsNotNull(exercises);
    }
    
    [TestMethod]
    public void AddSelectedExercise()
    {
        SelectedExercise selectedExercise = new()
        {
            UserId = 1,
            ExerciseId = 1
        };
        bool added = exerciseContainer.AddSelectedExercise(selectedExercise);
        Assert.IsTrue(added);
    }
    
    [TestMethod]
    public void DeleteSelectedExercise()
    {
        SelectedExercise selectedExercise = new()
        {
            UserId = 1,
            ExerciseId = 1
        };
        bool deleted = exerciseContainer.DeleteSelectedExercise(selectedExercise);
        Assert.IsFalse(deleted);
    }
    
    [TestMethod]
    public void GetSelectedExercises()
    {
        User user = new()
        {
            Id = 1
        };
        List<SelectedExercise> exercises = exerciseContainer.GetSelectedExercises(user);
        Assert.IsNotNull(exercises);
        Assert.AreEqual(user.Id, exercises[0].UserId);
    }

    [TestMethod]
    public void GetUnselectedExercises()
    {
        User user = new()
        {
            Id = 1
        };
        List<Exercise> exercises = exerciseContainer.GetUnselectedExercises(user);
        Assert.IsNotNull(exercises);
        Assert.AreEqual(user.Id, exercises[0].UserId);
    }
}