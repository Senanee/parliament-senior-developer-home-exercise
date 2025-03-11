using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
    }
}
