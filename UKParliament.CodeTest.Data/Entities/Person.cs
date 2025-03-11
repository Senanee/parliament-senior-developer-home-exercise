using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Data.Entities;
public class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public Department Department { get; set; }
}
