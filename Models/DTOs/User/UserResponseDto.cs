namespace Models.DTOs.User;

public class UserResponseDto : BaseResponse
{
    public List<UsersDto>? Users { get; set; }
}