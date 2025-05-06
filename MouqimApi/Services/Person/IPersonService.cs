using Models.DTOs.Person;

namespace MouqimApi.Services.Person;

public interface IPersonService
{
    Task<PersonResponseDto> GetAllPersons();
    Task<PersonResponseDto> GetPersonById(int id);
    Task<PersonResponseDto> AddPerson(AddPersonDto dto);
    Task<PersonResponseDto> UpdatePerson(UpdatePersonDto dto);
    Task<PersonResponseDto> DeletePerson(int id);
}