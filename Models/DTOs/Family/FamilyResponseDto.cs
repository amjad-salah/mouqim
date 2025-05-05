namespace Models.DTOs.Family;

public class FamilyResponseDto : BaseResponse
{
    public List<FamiliesDto>? Families { get; set; }
    public FamilyDto? Family { get; set; }
}