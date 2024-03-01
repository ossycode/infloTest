using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

/// <summary>
/// Represents a view model for displaying a list of users.
/// </summary>
public class UserListViewModel
{
    /// <summary>
    /// Gets or sets the list of user items in the view model.
    /// </summary>
    public List<UserListItemViewModel> Items { get; set; } = new();
}



/// <summary>
/// Represents a view model for displaying a single user item in a list.
/// </summary>
public class UserListItemViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the forename of the user.
    /// </summary>
    [Required(ErrorMessage = "Forename can't be blank")]
    public string? Forename { get; set; }

    /// <summary>
    /// Gets or sets the surname of the user.
    /// </summary>
    [Required(ErrorMessage = "Surname  can't be blank")]
    public string? Surname { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the log entries associated with the user.
    /// </summary>
    public IEnumerable<LogEntry>? LogEntries { get; set; }


    /// <summary>
    /// Converts the current object of CreateUserViewModel into a new object of User type
    /// </summary>
    /// <returns>A new User object.</returns>
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
