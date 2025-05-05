namespace Models.DTOs.Person;

public record UpdatePersonDto(
    int Id,
    string FullName,
    string PhoneNo,
    string NationalNo,
    DateTime BirthDate,
    Gender Gender,
    SocialStatus Status,
    int FamilyId,
    RelationType RelationType,
    int OccupationId,
    int EducationLevelId
);