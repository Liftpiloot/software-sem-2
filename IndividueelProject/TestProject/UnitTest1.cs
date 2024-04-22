using Iron_Domain;
using Iron_Interface;
using IronDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestProject;


[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void Register()
    {
        IDbUser db = new DbUserTest();
        UserContainer userContainer = new(db);
        DateTime date = new(2000, 1, 1);
        userContainer.AddUser(new User("Name", "Email@Email.com", "password", date, 80));
    }
    
    [TestMethod]
    public void Login()
    {
        IDbUser db = new DbUserTest();
        UserContainer userContainer = new(db);
        var user = new User
        {
            Email = "Email@Email.com",
            UserName = "Name",
            PasswordHash = "password"
        };
    }
}