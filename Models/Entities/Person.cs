using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("persons")]
public class Person : BaseEntity
{
    [Column("full_name", TypeName = "varchar(255)")]
    public string FullName { get; set; } = string.Empty;

    [Column("phone_no", TypeName = "varchar(20)")]
    public string PhoneNo { get; set; } = string.Empty;

    [Column("national_no", TypeName = "varchar(100)")]
    public string NationalNo { get; set; } = string.Empty;

    [Column("birth_date")] public DateTime BirthDate { get; set; }
    [Column("gender")] public Gender Gender { get; set; }
    [Column("status")] public SocialStatus Status { get; set; }
    [Column("family_id")] public int FamilyId { get; set; }
    [ForeignKey("FamilyId")] public virtual Family? Family { get; set; }
    [Column("relation_type")] public RelationType RelationType { get; set; }
    [Column("education_level_id")] public int EducationLevelId { get; set; }
    [Column("occupation_id")] public int OccupationId { get; set; }
    [ForeignKey("EducationLevelId")] public EducationLevel? EducationLevel { get; set; }
    [ForeignKey("OccupationId")] public Occupation? Occupation { get; set; }
}