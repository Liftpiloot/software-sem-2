using Iron_DAL.DTO;
using Iron_Interface;

namespace TestProject;

public class DbUserTest : IDbUser
{
    
    public int AddUser(UserDto? user)
    {
        return user != null ? 1 : 0;
    }

    public UserDto? Login(UserDto? user)
    {
        if (user != null)
        {
            return user with { Id = 1, Weight = 80 };
        }
        return null;
    }

    public bool EditWeight(int userId, decimal result)
    {
        throw new NotImplementedException();
    }
}