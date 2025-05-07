using System.Security.Claims;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.User;
using MouqimApi.Data;
using Microsoft.AspNetCore.Authentication;

namespace MouqimApi.Services.User;

public class UserService(
    MouqimDbContext context,
    IValidator<LoginDto> loginValidator,
    IValidator<AddUserDto> addValidator,
    IValidator<UpdateUserDto> updateValidator,
    IValidator<ResetPasswordDto> resetValidator) : IUserService
{
    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var validator = await loginValidator.ValidateAsync(loginDto);

        if (!validator.IsValid)
        {
            var errors = string.Join(", ", validator.Errors.Select(e => e.ErrorMessage));

            return new LoginResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var user = await context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

        if (user == null)
            return new LoginResponseDto
            {
                Success = false,
                Message = "Username or password is incorrect"
            };

        var isMatch = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

        if (!isMatch)
            return new LoginResponseDto
            {
                Success = false,
                Message = "Username or password is incorrect"
            };

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful",
            FullName = user.FullName,
            Role = Enum.GetName(user.Role.GetType(), user.Role)!
        };
    }

    public async Task<UserResponseDto> AddUsee(AddUserDto addUserDto)
    {
        var validator = await addValidator.ValidateAsync(addUserDto);

        if (!validator.IsValid)
        {
            var errors = string.Join(", ", validator.Errors.Select(e => e.ErrorMessage));

            return new UserResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingUser = await context.Users.AnyAsync(u => u.Username == addUserDto.Username);

        if (existingUser)
            return new UserResponseDto
            {
                Success = false,
                Message = "Username already exists"
            };

        var newUser = addUserDto.Adapt<Models.Entities.User>();

        newUser.Password = BCrypt.Net.BCrypt.HashPassword(addUserDto.Password);

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return new UserResponseDto
        {
            Success = true,
            Message = "User added successfully"
        };
    }

    public async Task<UserResponseDto> GetUsers()
    {
        var users = await context.Users.AsNoTracking()
            .ProjectToType<UsersDto>()
            .ToListAsync();

        return new UserResponseDto
        {
            Success = true,
            Users = users
        };
    }

    public async Task<UserResponseDto> UpdateUser(UpdateUserDto updateUserDto)
    {
        var validator = await updateValidator.ValidateAsync(updateUserDto);

        if (!validator.IsValid)
        {
            var errors = string.Join(", ", validator.Errors.Select(e => e.ErrorMessage));

            return new UserResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id);

        if (existingUser == null)
            return new UserResponseDto
            {
                Success = false,
                Message = "User not found"
            };

        existingUser.FullName = updateUserDto.FullName;
        existingUser.Role = updateUserDto.Role;
        existingUser.Username = updateUserDto.Username;

        await context.SaveChangesAsync();

        return new UserResponseDto
        {
            Success = true,
            Message = "User updated successfully"
        };
    }

    public async Task<UserResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var validator = await resetValidator.ValidateAsync(resetPasswordDto);

        if (!validator.IsValid)
        {
            var errors = string.Join(", ", validator.Errors.Select(e => e.ErrorMessage));

            return new UserResponseDto
            {
                Success = false,
                Message = errors
            };
        }

        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == resetPasswordDto.Id);

        if (existingUser == null)
            return new UserResponseDto
            {
                Success = false,
                Message = "User not found"
            };

        existingUser.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);

        await context.SaveChangesAsync();

        return new UserResponseDto
        {
            Success = true,
            Message = "Password reset successfully"
        };
    }

    public async Task<UserResponseDto> DeleteUser(int id)
    {
        var existingUser = await context.Users.FindAsync(id);

        if (existingUser == null)
            return new UserResponseDto
            {
                Success = false,
                Message = "User not found"
            };

        context.Users.Remove(existingUser);
        await context.SaveChangesAsync();

        return new UserResponseDto
        {
            Success = true,
            Message = "User deleted successfully"
        };
    }
}