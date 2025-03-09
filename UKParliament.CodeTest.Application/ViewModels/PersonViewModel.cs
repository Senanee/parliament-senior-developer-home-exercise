
using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Application.ViewModels;

public record PersonViewModel(
    int Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] DateTime DateOfBirth,
    [Required] int DepartmentId);