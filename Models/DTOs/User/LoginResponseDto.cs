namespace Models.DTOs.User;

public class LoginResponseDto : BaseResponse
{
    public string? FullName { get; set; }
    public string? Role { get; set; }
}