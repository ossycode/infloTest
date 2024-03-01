using System.ComponentModel.DataAnnotations;

namespace UserManagement.UI.ViewModels;

public class UserListViewModel
{
    public List<UserViewModel> Items { get; set; } = new();

    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}

public class UserViewModel
{
    public long Id { get; set; }

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


    public List<LogEntries>? LogEntries { get; set; }

}
