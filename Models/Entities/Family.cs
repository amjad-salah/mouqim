using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("families")]
public class Family : BaseEntity
{
    [Column("family_name", TypeName = "varchar(255)")]
    public string FamilyName { get; set; } = string.Empty;

    [Column("state")] public FamilyState State { get; set; }

    [Column("neighbourhood", TypeName = "varchar(255)")]
    public string Neighbourhood { get; set; } = string.Empty;

    [Column("income_status")] public IncomeStatus IncomeStatus { get; set; }
    [Column("housing_type")] public HousingType HousingType { get; set; }
    [Column("register_date")] public DateTime RegisterDate { get; set; }
    [Column("deactivated_date")] public DateTime? DeactivatedDate { get; set; }
    [Column("deactivation_reason")] public string? DeactivationReason { get; set; }
    public virtual List<Person>? Persons { get; set; }
}