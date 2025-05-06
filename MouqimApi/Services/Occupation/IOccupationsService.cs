using Models.DTOs.Occupation;

namespace MouqimApi.Services.Occupation;

public interface IOccupationsService
{
    Task<OccupationResponseDto> GetAllOccupations();
    Task<OccupationResponseDto> GetOccupationById(int id);
    Task<OccupationResponseDto> AddOccupation(AddOccupationDto dto);
    Task<OccupationResponseDto> UpdateOccupation(UpdateOccupationDto dto);
    Task<OccupationResponseDto> DeleteOccupation(int id);
}