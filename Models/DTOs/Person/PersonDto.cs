using Models.DTOs.Family;

namespace Models.DTOs.Person;

public record PersonDto(
    int Id,
    string FullName,
    string PhoneNo,
    string NationalNo,
    DateTime BirthDate,
    Gender Gender,
    SocialStatus Status,
    FamiliesDto? Family,
    RelationType RelationType
    //Occupation
    //Education
);