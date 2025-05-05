namespace Models.DTOs.Person;

public class PersonResponseDto : BaseResponse
{
    public List<PersonsDto>? Persons { get; set; }
    public PersonDto? Person { get; set; }
}