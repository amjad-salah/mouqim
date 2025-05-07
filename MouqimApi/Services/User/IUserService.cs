using Models.DTOs;
using Models.DTOs.User;

namespace MouqimApi.Services.User;

public interface IUserService
{
    Task<LoginResponseDto> Login(LoginDto loginDto);
    Task<UserResponseDto> AddUsee(AddUserDto addUserDto);
    Task<UserResponseDto> GetUsers();
    Task<UserResponseDto> UpdateUser(UpdateUserDto updateUserDto);
    Task<UserResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<UserResponseDto> DeleteUser(int id);
}