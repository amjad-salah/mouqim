using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("occupations")]
public class Occupation : BaseEntity
{
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    public virtual List<Person>? Persons { get; set; }
}