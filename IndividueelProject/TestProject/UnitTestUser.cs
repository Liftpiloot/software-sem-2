using Iron_Domain;
using Iron_Interface;
using IronDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestProject;


[TestClass]
public class UnitTestUser
{
    private static readonly IDbUser Db = new DbUserTest();
    private readonly UserContainer _userContainer = new(Db);
    
    [TestMethod]
    public void Register()
    {
        DateTime date = new(2000, 1, 1);
        int id = _userContainer.AddUser(new User("Name", "Email@Email.com", "password", date, 80));
        Assert.AreEqual(1, id);
    }
    
    [TestMethod]
    public void Login()
    {
        var user = new User
        {
            Email = "Email@Email.com",
            UserName = "Name",
            PasswordHash = "password"
        };
        User? loggedInUser = _userContainer.Login(user);
        Assert.IsNotNull(loggedInUser);
        Assert.AreEqual(1, loggedInUser.Id);
        Assert.AreEqual(80, loggedInUser.Weight);
    }
}