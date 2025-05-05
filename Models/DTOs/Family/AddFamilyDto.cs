namespace Models.DTOs.Family;

public record AddFamilyDto(
    string FamilyName,
    FamilyState State,
    string Neighbourhood,
    IncomeStatus IncomeStatus,
    HousingType HousingType
);