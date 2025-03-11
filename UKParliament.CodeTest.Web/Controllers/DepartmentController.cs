using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
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
        private readonly IDepartmentConversion _departmentConversion;

        public DepartmentController(IDepartmentService departmentService, IDepartmentConversion departmentConversion)
        {
            _departmentService = departmentService;
            _departmentConversion = departmentConversion;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            if (!departments.Any())
                return NotFound("No departments detected in the database");

            var list = _departmentConversion.ToViewModelList(departments);
            return list.Any() ? Ok(list) : NotFound("No departments found");
        }
    }
}