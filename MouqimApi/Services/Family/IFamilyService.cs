using Models.DTOs.Family;

namespace MouqimApi.Services.Family;

public interface IFamilyService
{
    Task<FamilyResponseDto> GetFamilies();
    Task<FamilyResponseDto> GetFamilyById(int id);
    Task<FamilyResponseDto> AddFamily(AddFamilyDto dto);
    Task<FamilyResponseDto> UpdateFamily(UpdateFamilyDto dto);
    Task<FamilyResponseDto> DeleteFamily(int id);
}