using UserManagement.Models;

namespace UserManagement.WebAPI.DTO;

/// <summary>
/// Represents a data transfer object (DTO) for a user.
/// </summary>
public class UserDTO
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the forename of the user.
    /// </summary>
    public string? Forename { get; set; }

    /// <summary>
    /// Gets or sets the surname of the user.
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the log entries associated with the user.
    /// </summary>
    public List<LogEntry>? LogEntries { get; set; }
}


