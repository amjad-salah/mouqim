namespace Models.DTOs.EducationLevel;

public class EducationLevelResponseDto : BaseResponse
{
    public List<EducationLevelsDto>? EducationLevels { get; set; }
    public EductionLevelDto? EductionLevel { get; set; }
}