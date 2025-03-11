using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Services.Interface;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Tests.Web.Controllers
{
    public class PersonControllerTests
    {
        private readonly IPersonService _personService;
        private readonly IPersonConversion _personConversion;
        private readonly IValidationService _validationService;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _personService = A.Fake<IPersonService>();
            _personConversion = A.Fake<IPersonConversion>();
            _validationService = A.Fake<IValidationService>();
            _controller = new PersonController(_personService, _personConversion, _validationService);
        }

        [Fact]
        public async Task GetPeople_ReturnsOkResult_WithListOfPeople()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 , Email = "John.Doe@test.com"},
                new Person { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 2, 2), DepartmentId = 2, Email = "Jane.Smith@test.com" }
            };
            var personViewModels = new List<PersonViewModel>
            {
                new PersonViewModel() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" },
                new PersonViewModel() { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 2, 2), DepartmentId = 2, Email = "John.Doe@test.com" }
            };

            A.CallTo(() => _personService.GetAllPeopleAsync()).Returns(people);
            A.CallTo(() => _personConversion.ToViewModelList(people)).Returns(personViewModels);

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
            A.CallTo(() => _personService.GetAllPeopleAsync()).Returns(new List<Person>());

            // Act
            var result = await _controller.GetPeople();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("No persons detected in the database");
        }

        [Fact]
        public async Task GetPerson_ReturnsOkResult_WithPerson()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var personViewModel = new PersonViewModel() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };

            A.CallTo(() => _personService.GetPersonByIdAsync(1)).Returns(person);
            A.CallTo(() => _personConversion.ToViewModel(person)).Returns(personViewModel);

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
            A.CallTo(() => _personService.GetPersonByIdAsync(10)).Returns<Person>(null!);

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
            var personViewModel = new PersonViewModel() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var response = new Response { Flag = true };

            A.CallTo(() => _personConversion.ToEntity(personViewModel)).Returns(person);
            A.CallTo(() => _personService.AddPersonAsync(person)).Returns(response);

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
            _controller.ModelState.AddModelError("FirstName", "First name is required");

            // Act
            var result = await _controller.AddPerson(new PersonViewModel() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" });

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsOkResult_WhenPersonIsUpdated()
        {
            // Arrange
            var personViewModel = new PersonViewModel() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };
            var response = new Response { Flag = true };

            A.CallTo(() => _personConversion.ToEntity(personViewModel)).Returns(person);
            A.CallTo(() => _personService.UpdatePersonAsync(person)).Returns(response);

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
            _controller.ModelState.AddModelError("FirstName", "First name is required");

            // Act
            var result = await _controller.UpdatePerson(new PersonViewModel() { Id = 1, FirstName = string.Empty, LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" });

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
            var okResult = result as OkObjectResult;
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
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(response);
        }
    }
}