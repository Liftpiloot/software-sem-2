using Iron_DAL.DTO;
using Iron_Domain;
using Iron_Interface;
using IronDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject;

[TestClass]
public class UnitTestExercise
{
    private ExerciseContainer _exerciseContainer;
    private DbExerciseTest _dbExerciseTest;
    
    [TestInitialize]
    public void SetUp()
    {
        _dbExerciseTest = new DbExerciseTest();
        _exerciseContainer = new ExerciseContainer(_dbExerciseTest);
    }
    
    [TestMethod]
        public void AddExercise_ShouldReturnValidId()
        {
            var exercise = new Exercise(1, 1, "Test Exercise", "Description", "/images/default.png");
            var result = _exerciseContainer.AddExercise(exercise);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DeleteExercise_ShouldReturnTrue()
        {
            var user = new User(1, "Test", "Test", "Test", new DateTime(1990, 1, 1), 0);
            var exercise = new Exercise(1, 1, "Test Exercise", "Description", "/images/default.png");
            var result = _exerciseContainer.DeleteExercise(user, exercise);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetExerciseFromId_ShouldReturnExercise()
        {
            var result = _exerciseContainer.GetExerciseFromId(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void GetExerciseFromId_ShouldReturnNull()
        {
            var result = _exerciseContainer.GetExerciseFromId(0);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetExercises_ShouldReturnExercisesList()
        {
            var user = new User(1, "Test", "Test", "Test", new DateTime(1990, 1, 1), 0);
            var result = _exerciseContainer.GetExercises(user);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void AddSelectedExercise_ShouldReturnTrue()
        {
            var selectedExercise = new SelectedExercise(1, 1);
            var result = _exerciseContainer.AddSelectedExercise(selectedExercise);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteSelectedExercise_ShouldReturnTrue()
        {
            var selectedExercise = new SelectedExercise(1, 1);
            var result = _exerciseContainer.DeleteSelectedExercise(selectedExercise);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetSelectedExercises_ShouldReturnSelectedExercisesList()
        {
            var user = new User(1, "Test", "Test", "Test", new DateTime(1990, 1, 1), 0);
            var result = _exerciseContainer.GetSelectedExercises(user);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetUnselectedExercises_ShouldReturnUnselectedExercisesList()
        {
            var user = new User(1, "Test", "Test", "Test", new DateTime(1990, 1, 1), 0);
            var result = _exerciseContainer.GetUnselectedExercises(user);
            Assert.AreEqual(2, result.Count);
        }
}