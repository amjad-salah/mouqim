using Models.DTOs.Person;

namespace Models.DTOs.Family;

public record FamilyDto(
    int Id,
    string FamilyName,
    FamilyState State,
    string Neighbourhood,
    IncomeStatus IncomeStatus,
    HousingType HousingType,
    DateTime RegisterDate,
    DateTime DeactivatedDate,
    string? DeactivationReason,
    List<PersonsDto>? Persons
);