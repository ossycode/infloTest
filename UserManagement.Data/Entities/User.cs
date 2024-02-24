using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [StringLength(40)]
    public string? Forename { get; set; } = default!;

    [StringLength(40)]
    public string? Surname { get; set; } = default!;

    [StringLength(40)]
    public string? Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
