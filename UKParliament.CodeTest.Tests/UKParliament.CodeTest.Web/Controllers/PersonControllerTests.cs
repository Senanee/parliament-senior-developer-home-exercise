using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Service.Interface;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Tests.Web.Controllers
{
    public class PersonControllerTests
    {
        private readonly IPersonService _personService;
        private readonly IValidationService _validationService;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _personService = A.Fake<IPersonService>();
            _validationService = A.Fake<IValidationService>();
            _controller = new PersonController(_personService, _validationService);
        }

        [Fact]
        public async Task GetPeople_ReturnsOkResult_WithListOfPeople()
        {
            // Arrange
            var people = new List<PersonViewModel>
            {
                new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" },
                new PersonViewModel { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 2, 2), DepartmentId = 2, Email = "Jane.Smith@test.com" }
            };

            A.CallTo(() => _personService.GetAllPeopleAsync()).Returns(people);

            // Act
            var result = await _controller.GetPeople();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returnValue = okResult.Value as List<PersonViewModel>;
            returnValue.Should().NotBeNull();
            returnValue.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetPeople_ReturnsNotFound_WhenNoPeopleExist()
        {
            // Arrange
            A.CallTo(() => _personService.GetAllPeopleAsync()).Returns(new List<PersonViewModel>());

            // Act
            var result = await _controller.GetPeople();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("No person found");
        }

        [Fact]
        public async Task GetPerson_ReturnsOkResult_WithPerson()
        {
            // Arrange
            var person = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };

            A.CallTo(() => _personService.GetPersonByIdAsync(1)).Returns(person);

            // Act
            var result = await _controller.GetPerson(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returnValue = okResult.Value as PersonViewModel;
            returnValue.Should().NotBeNull();
            returnValue.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetPerson_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _personService.GetPersonByIdAsync(10)).Returns<PersonViewModel>(null!);

            // Act
            var result = await _controller.GetPerson(10);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task AddPerson_ReturnsCreatedAtActionResult_WhenPersonIsAdded()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var response = new Response { Flag = true };

            A.CallTo(() => _validationService.Validate(personViewModel)).Returns(new List<ValidationResult>());
            A.CallTo(() => _personService.AddPersonAsync(personViewModel)).Returns(response);

            // Act
            var result = await _controller.AddPerson(personViewModel);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.Value.Should().Be(response);
        }

        [Fact]
        public async Task AddPerson_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doetest.com" };
            var validationResults = new List<ValidationResult> { new ValidationResult("First name is required") };

            A.CallTo(() => _validationService.Validate(personViewModel)).Returns(validationResults);

            // Act
            var result = await _controller.AddPerson(personViewModel);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsOkResult_WhenPersonIsUpdated()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var response = new Response { Flag = true };

            A.CallTo(() => _validationService.Validate(personViewModel)).Returns(new List<ValidationResult>());
            A.CallTo(() => _personService.UpdatePersonAsync(personViewModel)).Returns(response);

            // Act
            var result = await _controller.UpdatePerson(personViewModel);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(response);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = string.Empty, LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var validationResults = new List<ValidationResult> { new ValidationResult("First name is required") };

            A.CallTo(() => _validationService.Validate(personViewModel)).Returns(validationResults);

            // Act
            var result = await _controller.UpdatePerson(personViewModel);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeletePerson_ReturnsOkResult_WhenPersonIsDeleted()
        {
            // Arrange
            var response = new Response { Flag = true };

            A.CallTo(() => _personService.DeletePersonAsync(1)).Returns(response);

            // Act
            var result = await _controller.DeletePerson(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(response);
        }

        [Fact]
        public async Task DeletePerson_ReturnsBadRequest_WhenPersonIsNotDeleted()
        {
            // Arrange
            var response = new Response { Flag = false };

            A.CallTo(() => _personService.DeletePersonAsync(1)).Returns(response);

            // Act
            var result = await _controller.DeletePerson(1);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(response);
        }
    }
}
