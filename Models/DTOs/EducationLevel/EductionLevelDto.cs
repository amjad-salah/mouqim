using Models.DTOs.Person;

namespace Models.DTOs.EducationLevel;

public record EductionLevelDto(int Id, string Name, List<PersonDto>? Persons);