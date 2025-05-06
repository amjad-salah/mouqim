using Models.DTOs.EducationLevel;

namespace MouqimApi.Services.EducationLevel;

public interface IEducationLevelService
{
    Task<EducationLevelResponseDto> GetAllEducationLevels();
    Task<EducationLevelResponseDto> GetEducationLevelById(int id);
    Task<EducationLevelResponseDto> AddEducationLevel(AddEducationLevelDto dto);
    Task<EducationLevelResponseDto> UpdateEducationLevel(UpdateEducationLevelDto dto);
    Task<EducationLevelResponseDto> DeleteEducationLevel(int id);
}