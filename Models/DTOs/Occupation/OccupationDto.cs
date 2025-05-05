using Models.DTOs.Person;

namespace Models.DTOs.Occupation;

public record OccupationDto(
    int Id,
    string Name,
    List<PersonDto>? Persons
);