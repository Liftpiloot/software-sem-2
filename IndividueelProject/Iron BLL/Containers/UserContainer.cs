using System.Diagnostics.CodeAnalysis;
using Iron_DAL;
using Iron_DAL.DTO;
using Iron_Domain;
using Iron_Interface;

namespace IronDomain;

public class UserContainer
{
    private readonly IDbUser _dbUser;

    public UserContainer(IDbUser db)
    {
        _dbUser = db;
    }

    public int AddUser(User user)
    {
        UserDto userDto = new()
        {
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            DateOfBirth = user.DateOfBirth,
            Weight = user.Weight
        };
        int returnedUser = _dbUser.AddUser(userDto);
        return returnedUser;
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
        User newUser = new(returnedUser.Id, returnedUser.UserName, returnedUser.Email, returnedUser.PasswordHash, returnedUser.DateOfBirth, returnedUser.Weight);
        return newUser;
    }
    
}