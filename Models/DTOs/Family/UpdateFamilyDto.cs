namespace Models.DTOs.Family;

public record UpdateFamilyDto(
    int Id,
    string FamilyName,
    FamilyState State,
    string Neighbourhood,
    IncomeStatus IncomeStatus,
    HousingType HousingType,
    DateTime RegisterDate,
    DateTime? DeactivatedDate,
    string? DeactivationReason
);