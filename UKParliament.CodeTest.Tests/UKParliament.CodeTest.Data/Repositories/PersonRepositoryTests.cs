using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Data.Repositories
{
    public class PersonRepositoryTests
    {
        private readonly DbContextOptions<PersonManagerContext> _dbContextOptions;

        public PersonRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: $"PersonManager_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPeople()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            context.People.AddRange(new Person { FirstName = "John", LastName = "Doe" }, new Person { FirstName = "Jane", LastName = "Doe" });
            await context.SaveChangesAsync();

            var repository = new PersonRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var person = new Person { FirstName = "John", LastName = "Doe" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repository = new PersonRepository(context);

            // Act
            var result = await repository.GetByIdAsync(person.Id);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var repository = new PersonRepository(context);

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddPerson()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var repository = new PersonRepository(context);
            var person = new Person { FirstName = "John", LastName = "Doe" };

            // Act
            var result = await repository.AddAsync(person);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            context.People.Should().ContainSingle(p => p.FirstName == "John");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePerson()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var person = new Person { FirstName = "John", LastName = "Doe" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repository = new PersonRepository(context);
            person.LastName = "Smith";

            // Act
            await repository.UpdateAsync(person);

            // Assert
            var updatedPerson = await context.People.FindAsync(person.Id);
            updatedPerson.Should().NotBeNull();
            updatedPerson!.LastName.Should().Be("Smith");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePerson()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var person = new Person { FirstName = "John", LastName = "Doe" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repository = new PersonRepository(context);

            // Act
            await repository.DeleteAsync(person.Id);

            // Assert
            context.People.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByAsync_ShouldReturnPerson_WhenPredicateMatches()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var person = new Person { FirstName = "John", LastName = "Doe" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repository = new PersonRepository(context);

            // Act
            var result = await repository.GetByAsync(p => p.FirstName == "John");

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetByAsync_ShouldReturnNull_WhenPredicateDoesNotMatch()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var repository = new PersonRepository(context);

            // Act
            var result = await repository.GetByAsync(p => p.FirstName == "NonExistent");

            // Assert
            result.Should().BeNull();
        }
    }
}