using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Application.ViewModels
{
    public record DepartmentViewModel()
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
    }
}