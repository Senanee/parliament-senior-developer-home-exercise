using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Data;

public class Person
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    public Department Department { get; set; }

    // Add any additional properties here
}