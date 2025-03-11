using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Services.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    }
}