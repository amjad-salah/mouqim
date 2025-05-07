using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

[Table("users")]
public class User : BaseEntity
{
    [Column("full_name", TypeName = "varchar(255)")]
    public string FullName { get; set; } = string.Empty;

    [Column("username", TypeName = "varchar(255)")]
    public string Username { get; set; } = string.Empty;

    [Column("password", TypeName = "varchar(255)")]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}