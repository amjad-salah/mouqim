using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("education_levels")]
public class EducationLevel : BaseEntity
{
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    public virtual List<Person>? Persons { get; set; }
}