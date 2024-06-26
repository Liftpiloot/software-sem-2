﻿using FluentValidation.Results;
using Iron_Domain;
using Iron_Interface;
using Iron_Interface.DTO;
using IronDomain.Validators;

namespace IronDomain.Containers;

public class UserContainer
{
    private readonly IDbUser _dbUser;

    public UserContainer(IDbUser db)
    {
        _dbUser = db;
    }
    
    // method to convert user dto to user
    private static User ConvertToUser(UserDto userDto)
    {
        User user = new User(userDto.Id, userDto.UserName, userDto.Email, userDto.PasswordHash, userDto.DateOfBirth, userDto.Weight);
        return user;
    }
    
    // method to convert user to user dto
    private static UserDto ConvertToUserDto(User user)
    {
        UserDto userDto = new()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            DateOfBirth = user.DateOfBirth,
            Weight = user.Weight
        };
        return userDto;
    }
    
    public User? GetUser(int userId)
    {
        UserDto? userDto = _dbUser.GetUser(userId);
        if (userDto == null)
        {
            return null;
        }
        return ConvertToUser(userDto);
    }

    public (int, IList<ValidationFailure>) AddUser(User user)
    {
        var validator = new UserValidator();
        var result = validator.Validate(user);
        if (!result.IsValid)
        {
            return (-1, result.Errors);
        }
        
        UserDto userDto = ConvertToUserDto(user);
        int returnedUser = _dbUser.AddUser(userDto);
        return (returnedUser, new List<ValidationFailure>());
    }
    public User? Login(User user)
    {
        UserDto userDto = ConvertToUserDto(user);
        UserDto? returnedUser = _dbUser.Login(userDto);
        if (returnedUser == null)
        {
            return null;
        }
        return ConvertToUser(returnedUser);
    }


    public bool EditWeight(int userId, decimal result)
    {
        return _dbUser.EditWeight(userId, result);
    }

    public bool ChangePassword(int userId, string modelNewPassword)
    {
        return _dbUser.ChangePassword(userId, modelNewPassword);
    }
}