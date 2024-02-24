using System.ComponentModel.DataAnnotations;
using System;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

/// <summary>
/// Acts as a DTO for creating a new user
/// </summary>
public class CreateUserViewModel
{
    [Required(ErrorMessage = "Forename can't be blank")]
    public string? Forename { get; set; }

    [Required(ErrorMessage = "Surname  can't be blank")]
    public string? Surname { get; set; }

    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Date of Birth can't be blank")]
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }


    /// <summary>
    /// Converts the current object of CreateUserViewModel into a new object of User type
    /// </summary>
    /// <returns></returns>
    public User ToUser()
    {
        var user = new User { IsActive = IsActive };


        if (Forename != null)
        {
            user.Forename = Forename;
        }

        if (Surname != null)
        {
            user.Surname = Surname;
        }

        if (Email != null)
        {
            user.Email = Email;
        }

        if (DateOfBirth != null)
        {
            user.DateOfBirth = (DateTime)DateOfBirth;
        }

        return user;
    }
}
