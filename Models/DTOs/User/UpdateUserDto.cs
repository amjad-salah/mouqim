namespace Models.DTOs.User;

public record UpdateUserDto(int Id, string Username, string Password, string FullName, UserRole Role);