using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Data.Repositories
{
    public class DepartmentRepositoryTests
    {
        private readonly DbContextOptions<PersonManagerContext> _dbContextOptions;

        public DepartmentRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: $"PersonManager_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            context.Departments.AddRange(new Department { Name = "HR" }, new Department { Name = "IT" });
            await context.SaveChangesAsync();

            var repository = new DepartmentRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDepartment_WhenDepartmentExists()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var department = new Department { Name = "HR" };
            context.Departments.Add(department);
            await context.SaveChangesAsync();

            var repository = new DepartmentRepository(context);

            // Act
            var result = await repository.GetByIdAsync(department.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("HR");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            // Arrange
            await using var context = new PersonManagerContext(_dbContextOptions);
            var repository = new DepartmentRepository(context);

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }
    }
}
