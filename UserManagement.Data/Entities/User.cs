using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [StringLength(40)]
    [Required]
    public string? Forename { get; set; } = default!;

    [Required]
    [StringLength(40)]
    public string? Surname { get; set; } = default!;

    [Required]
    [StringLength(40)]
    public string? Email { get; set; } = default!;

    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

}
