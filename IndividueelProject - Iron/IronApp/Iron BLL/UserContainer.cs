using System.Diagnostics.CodeAnalysis;
using Iron_DAL;
using Iron_DAL.DTO;
using Iron_Domain;
namespace IronDomain;

public class UserContainer
{
    private readonly DbUser _dbUser = new();
    
    public User? AddUser(User user)
    {
        UserDto userDto = new()
        {
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            DateOfBirth = user.DateOfBirth,
            Weight = user.Weight
        };
        UserDto? returnedUser = _dbUser.AddUser(userDto);
        if (returnedUser == null)
        {
            return null;
        }
        User newUser = new(returnedUser.Id, returnedUser.UserName, returnedUser.Email, returnedUser.PasswordHash, returnedUser.DateOfBirth, returnedUser.Weight);
        return newUser;
    }
    public User? Login(User user)
    {
        UserDto userDto = new()
        {
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };
        UserDto? returnedUser = _dbUser.Login(userDto);
        if (returnedUser == null)
        {
            return null;
        }
        User newUser = new(returnedUser.UserName, returnedUser.Email, returnedUser.PasswordHash, returnedUser.DateOfBirth, returnedUser.Weight);
        return newUser;
    }
    
}