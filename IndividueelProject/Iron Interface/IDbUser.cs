using Iron_DAL.DTO;

namespace Iron_Interface;

public interface IDbUser
{
    public int AddUser(UserDto? user);
    public UserDto? Login(UserDto? user);
    bool EditWeight(int userId, decimal result);
}