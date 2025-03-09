using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Application.ViewModels
{
    public record DepartmentViewModel(
        int Id,
        [Required] string Name
        );
}
