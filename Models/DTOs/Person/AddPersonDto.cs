namespace Models.DTOs.Person;

public record AddPersonDto(
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