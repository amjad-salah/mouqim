namespace Models.DTOs.User;

public record AddUserDto(string Username, string Password, string FullName, UserRole Role);