namespace Models.DTOs.Occupation;

public class OccupationResponseDto : BaseResponse
{
    public List<OccupationsDto>? Occupations { get; set; }
    public OccupationDto? Occupation { get; set; }
}