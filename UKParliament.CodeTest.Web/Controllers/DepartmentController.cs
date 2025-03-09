using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Interface;

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
            var departments = await _departmentService.GetAllDepartmentsAsync();
            if (!departments.Any())
                return NotFound("No departments detected in the database");

            var (_, list) = DepartmentConversion.FromEntity(null!, departments);
            return list!.Any() ? Ok(list) : NotFound("No departments found");
        }
    }
}

