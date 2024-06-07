using Iron_Interface;
using Iron_Interface.DTO;

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
            if (user.PasswordHash == string.Empty)
            {
                return null;
            }
            if (user.UserName == string.Empty && user.Email == string.Empty)
            {
                return null;
            }
            return user with { Id = 1, Weight = 80 };
        }
        return null;
    }

    public bool EditWeight(int userId, decimal weight)
    {
        return userId != 0 && weight != 0;
    }

    public bool ChangePassword(int userId, string password)
    {
        return userId != 0 && !string.IsNullOrEmpty(password);
    }

    public UserDto? GetUser(int userId)
    {
        if (userId != 0)
        {
            return new UserDto
            {
                Id = userId,
                Weight = 80
            };
        }
        return null;
    }
}