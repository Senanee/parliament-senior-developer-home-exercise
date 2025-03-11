using FakeItEasy;
using FluentAssertions;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Service;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Services.Service
{
    public class DepartmentServiceTests
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentConversion _departmentConversion;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _departmentRepository = A.Fake<IDepartmentRepository>();
            _departmentConversion = A.Fake<IDepartmentConversion>();
            _departmentService = new DepartmentService(_departmentRepository, _departmentConversion);
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

            var departmentViewModels = new List<DepartmentViewModel>
            {
                new DepartmentViewModel{Id = 1, Name = "HR" },
                new DepartmentViewModel{Id = 2, Name = "IT" }
            };

            A.CallTo(() => _departmentRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Department>)departments));
            A.CallTo(() => _departmentConversion.ToViewModelList(departments)).Returns(departmentViewModels);

            // Act
            var result = await _departmentService.GetAllDepartmentsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(departmentViewModels);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnEmptyList_WhenNoDepartmentsExist()
        {
            // Arrange
            A.CallTo(() => _departmentRepository.GetAllAsync()).Returns(Task.FromResult(Enumerable.Empty<Department>()));
            A.CallTo(() => _departmentConversion.ToViewModelList(A<IEnumerable<Department>>._)).Returns(Enumerable.Empty<DepartmentViewModel>());

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
            var departmentViewModel = new DepartmentViewModel{ Id = 1, Name = "HR" }    ;

            A.CallTo(() => _departmentRepository.GetByIdAsync(1)).Returns(Task.FromResult(department));
            A.CallTo(() => _departmentConversion.ToViewModel(department)).Returns(departmentViewModel);

            // Act
            var result = await _departmentService.GetByIdAsync(1);

            // Assert
            result.Should().Be(departmentViewModel);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            // Arrange
            A.CallTo(() => _departmentRepository.GetByIdAsync(1)).Returns(Task.FromResult<Department>(null!));
            A.CallTo(() => _departmentConversion.ToViewModel(null!)).Returns(null!);

            // Act
            var result = await _departmentService.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }
    }
}
