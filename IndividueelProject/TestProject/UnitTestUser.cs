using Iron_Domain;
using Iron_Interface;
using IronDomain;
using IronDomain.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestProject;


[TestClass]
public class UnitTestUser
{
    private IDbUser _mockDbUser;
    private UserContainer _container;
    
    [TestInitialize]
        public void SetUp()
        {
            _mockDbUser = new DbUserTest();
            _container = new UserContainer(_mockDbUser);
        }

        [TestMethod]
        public void AddUser_ShouldReturnUserId()
        {
            // Arrange
            var user = new User(0, "testUser", "test@example.com", "hashedpassword", new DateTime(1990, 1, 1), 70);

            // Act
            var (result, errors) = _container.AddUser(user);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetUser_ShouldReturnUser()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = _container.GetUser(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(80, result.Weight);
        }

        [TestMethod]
        public void GetUser_ShouldReturnNullForInvalidUserId()
        {
            // Arrange
            var userId = 0;

            // Act
            var result = _container.GetUser(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Login_ShouldReturnUser()
        {
            // Arrange
            var user = new User(0, "testUser", "test@example.com", "hashedpassword", new DateTime(1990, 1, 1), 70);

            // Act
            var result = _container.Login(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(80, result.Weight);
        }

        [TestMethod]
        public void Login_ShouldReturnNullForInvalidUser()
        {
            // Arrange
            User user = new User(0, "", "", "", new DateTime(1990, 1, 1), 0);

            // Act
            var result = _container.Login(user);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EditWeight_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var newWeight = 75m;

            // Act
            var result = _container.EditWeight(userId, newWeight);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EditWeight_ShouldReturnFalseForInvalidInput()
        {
            // Arrange
            var userId = 0;
            var newWeight = 0m;

            // Act
            var result = _container.EditWeight(userId, newWeight);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ChangePassword_ShouldReturnTrue()
        {
            // Arrange
            var userId = 1;
            var newPassword = "newPassword";

            // Act
            var result = _container.ChangePassword(userId, newPassword);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ChangePassword_ShouldReturnFalseForInvalidInput()
        {
            // Arrange
            var userId = 0;
            var newPassword = "";

            // Act
            var result = _container.ChangePassword(userId, newPassword);

            // Assert
            Assert.IsFalse(result);
        }
}