using FakeItEasy;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Interface;
using UKParliament.CodeTest.Services.Service;
using UKParliament.CodeTest.Tests.Common;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Services.Service
{
    public class PersonServiceTests
    {
        private readonly IPersonRepository _personRepository;
        private readonly IValidationService _validationService;
        private readonly PersonService _personService;
        private readonly PersonManagerContext _context;

        public PersonServiceTests()
        {
            _context = InMemoryDbContextFactory.Create();
            _personRepository = A.Fake<IPersonRepository>();
            _validationService = A.Fake<IValidationService>();
            _personService = new PersonService(_personRepository, _validationService);
        }

        [Fact]
        public async Task GetAllPeopleAsync_ShouldReturnAllPeople()
        {
            // Arrange
            var people = new List<Person>
        {
            new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" },
            new Person { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 2, 2), DepartmentId = 2, Email = "jane.smith@test.com" }
        };

            A.CallTo(() => _personRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Person>)people));

            // Act
            var result = await _personService.GetAllPeopleAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(people);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personRepository.GetByIdAsync(1)).Returns(Task.FromResult(person));

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            result.Should().Be(person);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _personRepository.GetByIdAsync(1)).Returns(Task.FromResult<Person>(null));

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddPersonAsync_ShouldReturnResponseWithErrors_WhenValidationFails()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var validationResults = new List<ValidationResult> { new ValidationResult("First name is required") };

            A.CallTo(() => _validationService.Validate(person)).Returns(validationResults);

            // Act
            var result = await _personService.AddPersonAsync(person);

            // Assert
            result.Flag.Should().BeFalse();
            result.message.Should().Contain("First name is required");
        }

        [Fact]
        public async Task AddPersonAsync_ShouldReturnSuccessResponse_WhenPersonIsAdded()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _validationService.Validate(person)).Returns(new List<ValidationResult>());
            A.CallTo(() => _personRepository.AddAsync(person)).Returns(Task.FromResult(person));

            // Act
            var result = await _personService.AddPersonAsync(person);

            // Assert
            result.Flag.Should().BeTrue();
            result.message.Should().Contain("added to database successfully");
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldReturnResponseWithErrors_WhenValidationFails()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var validationResults = new List<ValidationResult> { new ValidationResult("First name is required") };

            A.CallTo(() => _validationService.Validate(person)).Returns(validationResults);

            // Act
            var result = await _personService.UpdatePersonAsync(person);

            // Assert
            result.Flag.Should().BeFalse();
            result.message.Should().Contain("First name is required");
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldReturnSuccessResponse_WhenPersonIsUpdated()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _validationService.Validate(person)).Returns(new List<ValidationResult>());
            A.CallTo(() => _personRepository.GetByIdAsync(person.Id)).Returns(Task.FromResult(person));
            A.CallTo(() => _personRepository.UpdateAsync(person)).Returns(Task.CompletedTask);

            // Act
            var result = await _personService.UpdatePersonAsync(person);

            // Assert
            result.Flag.Should().BeTrue();
            result.message.Should().Contain("is updated successfully");
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnSuccessResponse_WhenPersonIsDeleted()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personRepository.GetByIdAsync(person.Id)).Returns(Task.FromResult(person));
            A.CallTo(() => _personRepository.DeleteAsync(person.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _personService.DeletePersonAsync(person.Id);

            // Assert
            result.Flag.Should().BeTrue();
            result.message.Should().Contain("is deleted successfully");
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnErrorResponse_WhenPersonDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _personRepository.GetByIdAsync(1)).Returns(Task.FromResult<Person>(null));

            // Act
            var result = await _personService.DeletePersonAsync(1);

            // Assert
            result.Flag.Should().BeFalse();
            result.message.Should().Contain("Person not found");
        }
    }
}