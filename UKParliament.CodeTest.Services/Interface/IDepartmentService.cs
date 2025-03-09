using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    }
}