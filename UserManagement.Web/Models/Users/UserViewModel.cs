using System;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

/// <summary>
/// Acts as a DTO for a single user
/// </summary>
public class UserViewModel
{
    public long Id { get; set; }

    public string? Forename { get; set; }

    public string? Surname { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; }

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
