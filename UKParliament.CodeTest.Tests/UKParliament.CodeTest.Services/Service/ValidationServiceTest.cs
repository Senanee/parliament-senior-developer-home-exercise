using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Services.Service;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Services.Service
{
    public class ValidationServiceTests
    {
        private readonly ValidationService _validationService;

        public ValidationServiceTests()
        {
            _validationService = new ValidationService();
        }

        public class TestModel
        {
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }

            [Range(1, 100, ErrorMessage = "Age must be between 1 and 100")]
            public int Age { get; set; }
        }

        [Fact]
        public void Validate_ShouldReturnValidationErrors_WhenModelIsInvalid()
        {
            // Arrange
            var model = new TestModel { Name = "", Age = 101 };

            // Act
            var validationResults = _validationService.Validate(model);

            // Assert
            validationResults.Should().NotBeNull();
            validationResults.Should().HaveCount(2);
            validationResults.Should().Contain(v => v.ErrorMessage == "Name is required");
            validationResults.Should().Contain(v => v.ErrorMessage == "Age must be between 1 and 100");
        }

        [Fact]
        public void Validate_ShouldReturnNoValidationErrors_WhenModelIsValid()
        {
            // Arrange
            var model = new TestModel { Name = "John Doe", Age = 30 };

            // Act
            var validationResults = _validationService.Validate(model);

            // Assert
            validationResults.Should().NotBeNull();
            validationResults.Should().BeEmpty();
        }
    }
}