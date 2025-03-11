using FakeItEasy;
using FluentAssertions;
using System.Linq.Expressions;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Service;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Services.Service
{
    public class PersonServiceTests
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonConversion _personConversion;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _personRepository = A.Fake<IPersonRepository>();
            _personConversion = A.Fake<IPersonConversion>();
            _personService = new PersonService(_personRepository, _personConversion);
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

            var personViewModels = new List<PersonViewModel>
            {
                new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" },
                new PersonViewModel { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 2, 2), DepartmentId = 2, Email = "jane.smith@test.com" }
            };

            A.CallTo(() => _personRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Person>)people));
            A.CallTo(() => _personConversion.ToViewModelList(people)).Returns(personViewModels);

            // Act
            var result = await _personService.GetAllPeopleAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(personViewModels);
        }

        [Fact]
        public async Task GetAllPeopleAsync_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange
            A.CallTo(() => _personRepository.GetAllAsync()).Throws<Exception>();

            // Act
            Func<Task> act = async () => await _personService.GetAllPeopleAsync();

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Error occurred retrieving people");
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personRepository.GetByIdAsync(1)).Returns(Task.FromResult(person));
            A.CallTo(() => _personConversion.ToViewModel(person)).Returns(personViewModel);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            result.Should().Be(personViewModel);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange
            A.CallTo(() => _personRepository.GetByIdAsync(1)).Throws<Exception>();

            // Act
            Func<Task> act = async () => await _personService.GetPersonByIdAsync(1);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Error occurred retrieving entity");
        }

        [Fact]
        public async Task AddPersonAsync_ShouldReturnSuccessResponse_WhenPersonIsAdded()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personConversion.ToEntity(personViewModel)).Returns(person);
            A.CallTo(() => _personRepository.AddAsync(person)).Returns(Task.FromResult(person));

            // Act
            var result = await _personService.AddPersonAsync(personViewModel);

            // Assert
            result.Flag.Should().BeTrue();
            result.message.Should().Contain("added to database successfully");
        }

        [Fact]
        public async Task AddPersonAsync_ShouldReturnErrorResponse_WhenExceptionOccurs()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personConversion.ToEntity(personViewModel)).Returns(person);
            A.CallTo(() => _personRepository.AddAsync(person)).Throws<Exception>();

            // Act
            var result = await _personService.AddPersonAsync(personViewModel);

            // Assert
            result.Flag.Should().BeFalse();
            result.message.Should().Contain("Error occurred adding new entity");
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldReturnSuccessResponse_WhenPersonIsUpdated()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personConversion.ToEntity(personViewModel)).Returns(person);
            A.CallTo(() => _personRepository.GetByIdAsync(personViewModel.Id)).Returns(Task.FromResult(person));
            A.CallTo(() => _personRepository.UpdateAsync(person)).Returns(Task.CompletedTask);

            // Act
            var result = await _personService.UpdatePersonAsync(personViewModel);

            // Assert
            result.Flag.Should().BeTrue();
            result.message.Should().Contain("is updated successfully");
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldReturnErrorResponse_WhenExceptionOccurs()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personConversion.ToEntity(personViewModel)).Returns(person);
            A.CallTo(() => _personRepository.GetByIdAsync(personViewModel.Id)).Returns(Task.FromResult(person));
            A.CallTo(() => _personRepository.UpdateAsync(person)).Throws<Exception>();

            // Act
            var result = await _personService.UpdatePersonAsync(personViewModel);

            // Assert
            result.Flag.Should().BeFalse();
            result.message.Should().Contain("Error occurred updating person");
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
        public async Task DeletePersonAsync_ShouldReturnErrorResponse_WhenExceptionOccurs()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personRepository.GetByIdAsync(person.Id)).Returns(Task.FromResult(person));
            A.CallTo(() => _personRepository.DeleteAsync(person.Id)).Throws<Exception>();

            // Act
            var result = await _personService.DeletePersonAsync(person.Id);

            // Assert
            result.Flag.Should().BeFalse();
            result.message.Should().Contain("Error occurred Deleting entity");
        }

        [Fact]
        public async Task GetByAsync_ShouldReturnPerson_WhenPredicateMatches()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "john.doe@test.com" };

            A.CallTo(() => _personRepository.GetByAsync(A<Expression<Func<Person, bool>>>._)).Returns(Task.FromResult(person));

            // Act
            var result = await _personService.GetByAsync(p => p.FirstName == "John");

            // Assert
            result.Should().Be(person);
        }

        [Fact]
        public async Task GetByAsync_ShouldReturnNull_WhenPredicateDoesNotMatch()
        {
            // Arrange
            A.CallTo(() => _personRepository.GetByAsync(A<Expression<Func<Person, bool>>>._)).Returns(Task.FromResult<Person>(null!));

            // Act
            var result = await _personService.GetByAsync(p => p.FirstName == "NonExistent");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByAsync_ShouldThrowException_WhenErrorOccurs()
        {
            // Arrange
            A.CallTo(() => _personRepository.GetByAsync(A<Expression<Func<Person, bool>>>._)).Throws<Exception>();

            // Act
            Func<Task> act = async () => await _personService.GetByAsync(p => p.FirstName == "John");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Error occurred retrieving entity");
        }
    }
}
