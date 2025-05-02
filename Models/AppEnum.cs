namespace Models;

public enum Gender
{
    Male,
    Female
}

public enum FamilyState
{
    UnderRegistration,
    Approved,
    Rejected,
    Proposed
}

public enum RelationType
{
    Parent,
    Child,
    Spouse,
    Brother,
    Sister,
    Grandparent,
    Grandchild,
    Other
}

public enum SocialStatus
{
    Single,
    Married,
    Divorced,
    Widow
}

public enum HousingType
{
    Rent,
    Owned,
    Government,
    Guest,
    Temporarily,
    None
}

public enum IncomeStatus
{
    None,
    Low,
    Medium,
    High,
    VeryHigh
}