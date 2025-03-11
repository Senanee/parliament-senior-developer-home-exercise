using UKParliament.CodeTest.Application.ViewModels;

namespace UKParliament.CodeTest.Services.Service.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentViewModel>> GetAllDepartmentsAsync();
    }
}