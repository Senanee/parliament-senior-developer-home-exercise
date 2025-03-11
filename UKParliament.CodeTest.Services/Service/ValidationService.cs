using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Services.Service.Interface;

namespace UKParliament.CodeTest.Services.Service
{
    public class ValidationService : IValidationService
    {
        public List<ValidationResult> Validate<T>(T model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}