using System.ComponentModel.DataAnnotations;
using System;

namespace UserManagement.Web.Models.Users;

public class UpdateUserViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    public string? Forename { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}
