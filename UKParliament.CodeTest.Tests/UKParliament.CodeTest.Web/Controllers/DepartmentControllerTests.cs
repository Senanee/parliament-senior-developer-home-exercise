using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Service.Interface;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Tests.Web.Controllers
{
    public class DepartmentControllerTests
    {
        private readonly IDepartmentService _departmentService;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _departmentService = A.Fake<IDepartmentService>();
            _controller = new DepartmentController(_departmentService);
        }

        [Fact]
        public async Task GetDepartments_ReturnsOkResult_WithListOfDepartments()
        {
            // Arrange
            var departmentViewModels = new List<DepartmentViewModel>
            {
                new DepartmentViewModel() { Id = 1, Name = "HR" },
                new DepartmentViewModel() { Id = 2, Name = "IT" }
            };

            A.CallTo(() => _departmentService.GetAllDepartmentsAsync()).Returns(Task.FromResult((IEnumerable<DepartmentViewModel>)departmentViewModels));

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
            A.CallTo(() => _departmentService.GetAllDepartmentsAsync()).Returns(Task.FromResult(Enumerable.Empty<DepartmentViewModel>()));

            // Act
            var result = await _controller.GetDepartments();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("No departments found");
        }

        [Fact]
        public async Task GetDepartments_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            A.CallTo(() => _departmentService.GetAllDepartmentsAsync()).Throws<Exception>();

            // Act
            var result = await _controller.GetDepartments();

            // Assert
            var internalServerErrorResult = result.Result as ObjectResult;
            internalServerErrorResult.Should().NotBeNull();
            internalServerErrorResult.StatusCode.Should().Be(500);
            internalServerErrorResult.Value.Should().Be("An error occurred while retrieving departments");
        }

    }
}

