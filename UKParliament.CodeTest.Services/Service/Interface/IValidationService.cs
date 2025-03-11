using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Services.Service.Interface
{
    public interface IValidationService
    {
        List<ValidationResult> Validate<T>(T model);
    }
}