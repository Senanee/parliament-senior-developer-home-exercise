using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Application.ViewModels;

public record PersonViewModel()
{
    public int Id { get; init; }
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; init; } = string.Empty;
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; init; } = string.Empty;
    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
    public DateTime DateOfBirth { get; init; }
    [Required(ErrorMessage = "Department is required")]
    public int DepartmentId { get; init; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email format")]
    public string Email { get; init; } = string.Empty;
}