using FluentAssertions;
using global::UKParliament.CodeTest.Application.Conversions;
using global::UKParliament.CodeTest.Application.ViewModels;
using global::UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Entities;
using Xunit;

namespace UKParliament.CodeTest.Tests.UKParliament.CodeTest.Application.Conversions
{
    public class DepartmentConversionTests
    {
        private readonly DepartmentConversion _departmentConversion;

        public DepartmentConversionTests()
        {
            _departmentConversion = new DepartmentConversion();
        }

        [Fact]
        public void ToEntity_ReturnsDepartmentEntity_WhenViewModelIsValid()
        {
            // Arrange
            var departmentViewModel = new DepartmentViewModel(1, "HR");

            // Act
            var result = _departmentConversion.ToEntity(departmentViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(departmentViewModel.Id);
            result.Name.Should().Be(departmentViewModel.Name);
        }


        [Fact]
        public void ToViewModel_ReturnsDepartmentViewModel_WhenEntityIsValid()
        {
            // Arrange
            var department = new Department { Id = 1, Name = "HR" };

            // Act
            var result = _departmentConversion.ToViewModel(department);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(department.Id);
            result.Name.Should().Be(department.Name);
        }


        [Fact]
        public void ToViewModelList_ReturnsListOfDepartmentViewModels_WhenEntitiesAreValid()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            };

            // Act
            var result = _departmentConversion.ToViewModelList(departments);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.First().Id.Should().Be(departments.First().Id);
            result.First().Name.Should().Be(departments.First().Name);
        }

    }
}

