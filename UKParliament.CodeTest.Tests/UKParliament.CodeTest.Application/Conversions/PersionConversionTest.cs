using FluentAssertions;
using UKParliament.CodeTest.Application.Conversions;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;
using Xunit;

namespace UKParliament.CodeTest.Tests.Conversions
{
    public class PersonConversionTests
    {
        private readonly PersonConversion _personConversion;

        public PersonConversionTests()
        {
            _personConversion = new PersonConversion();
        }

        [Fact]
        public void ToEntity_ReturnsPersonEntity_WhenViewModelIsValid()
        {
            // Arrange
            var personViewModel = new PersonViewModel() { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };

            // Act
            var result = _personConversion.ToEntity(personViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(personViewModel.Id);
            result.FirstName.Should().Be(personViewModel.FirstName);
            result.LastName.Should().Be(personViewModel.LastName);
            result.DateOfBirth.Should().Be(personViewModel.DateOfBirth);
            result.DepartmentId.Should().Be(personViewModel.DepartmentId);
        }

        [Fact]
        public void ToViewModel_ReturnsPersonViewModel_WhenEntityIsValid()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1, Email = "John.Doe@test.com" };

            // Act
            var result = _personConversion.ToViewModel(person);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(person.Id);
            result.FirstName.Should().Be(person.FirstName);
            result.LastName.Should().Be(person.LastName);
            result.DateOfBirth.Should().Be(person.DateOfBirth);
            result.DepartmentId.Should().Be(person.DepartmentId);
        }

        [Fact]
        public void ToViewModelList_ReturnsListOfPersonViewModels_WhenEntitiesAreValid()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1), DepartmentId = 1 , Email = "John.Doe@test.com"},
                new Person { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1992, 2, 2), DepartmentId = 2 , Email = "Jane.Smith@test.com"}
            };

            // Act
            var result = _personConversion.ToViewModelList(people);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.First().Id.Should().Be(people.First().Id);
            result.First().FirstName.Should().Be(people.First().FirstName);
            result.First().LastName.Should().Be(people.First().LastName);
            result.First().DateOfBirth.Should().Be(people.First().DateOfBirth);
            result.First().DepartmentId.Should().Be(people.First().DepartmentId);
        }
    }
}