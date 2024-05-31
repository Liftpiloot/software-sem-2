using Iron_DAL.DTO;

namespace Iron_Interface;

public interface IDbUser
{
    int AddUser(UserDto? user);
    UserDto? Login(UserDto? user);
    bool EditWeight(int userId, decimal weight);
    bool ChangePassword(int userId, string password);
    UserDto? GetUser(int userId);
}