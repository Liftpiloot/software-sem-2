using Iron_DAL.DTO;
using Iron_Interface;

namespace TestProject;

public class DbUserTest : IDbUser
{
    public int AddUser(UserDto? user)
    {
        throw new NotImplementedException();
    }

    public UserDto? Login(UserDto? user)
    {
        throw new NotImplementedException();
    }
}