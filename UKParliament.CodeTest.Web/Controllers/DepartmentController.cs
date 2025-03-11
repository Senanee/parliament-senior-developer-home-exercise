using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Service.Interface;

namespace UKParliament.CodeTest.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetDepartments()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                return departments.Any() ? Ok(departments) : NotFound("No departments found");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving departments");
            }
        }
    }
}