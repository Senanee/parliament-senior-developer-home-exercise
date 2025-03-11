using FakeItEasy;
using FluentAssertions;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Service;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Services.Service
{
    public class DepartmentServiceTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _departmentRepository = A.Fake<IDepartmentRepository>();
            _departmentService = new DepartmentService(_departmentRepository);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            };

            A.CallTo(() => _departmentRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Department>)departments));

            // Act
            var result = await _departmentService.GetAllDepartmentsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(departments);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnEmptyList_WhenNoDepartmentsExist()
        {
            // Arrange
            A.CallTo(() => _departmentRepository.GetAllAsync()).Returns(Task.FromResult(Enumerable.Empty<Department>()));

            // Act
            var result = await _departmentService.GetAllDepartmentsAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDepartment_WhenDepartmentExists()
        {
            // Arrange
            var department = new Department { Id = 1, Name = "HR" };

            A.CallTo(() => _departmentRepository.GetByIdAsync(1)).Returns(Task.FromResult(department));

            // Act
            var result = await _departmentService.GetByIdAsync(1);

            // Assert
            result.Should().Be(department);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _departmentRepository.GetByIdAsync(1)).Returns(Task.FromResult<Department>(null!));

            // Act
            var result = await _departmentService.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }
    }
}
