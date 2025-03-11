using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Services.Interface;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Tests.Web.Controllers
{
    public class DepartmentControllerTests
    {
        private readonly IDepartmentService _departmentService;
        private readonly IDepartmentConversion _departmentConversion;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _departmentService = A.Fake<IDepartmentService>();
            _departmentConversion = A.Fake<IDepartmentConversion>();
            _controller = new DepartmentController(_departmentService, _departmentConversion);
        }

        [Fact]
        public async Task GetDepartments_ReturnsOkResult_WithListOfDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            };
            var departmentViewModels = new List<DepartmentViewModel>
            {
                new DepartmentViewModel(1, "HR"),
                new DepartmentViewModel(2, "IT")
            };

            A.CallTo(() => _departmentService.GetAllDepartmentsAsync()).Returns(departments);
            A.CallTo(() => _departmentConversion.ToViewModelList(departments)).Returns(departmentViewModels);

            // Act
            var result = await _controller.GetDepartments();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returnValue = okResult.Value as List<DepartmentViewModel>;
            returnValue.Should().NotBeNull();
            returnValue.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetDepartments_ReturnsNotFound_WhenNoDepartmentsExist()
        {
            // Arrange
            A.CallTo(() => _departmentService.GetAllDepartmentsAsync()).Returns(new List<Department>());

            // Act
            var result = await _controller.GetDepartments();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("No departments detected in the database");
        }

        [Fact]
        public async Task GetDepartments_ReturnsNotFound_WhenConversionFails()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            };

            A.CallTo(() => _departmentService.GetAllDepartmentsAsync()).Returns(departments);
            A.CallTo(() => _departmentConversion.ToViewModelList(departments)).Returns(new List<DepartmentViewModel>());

            // Act
            var result = await _controller.GetDepartments();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("No departments found");
        }
    }
}
