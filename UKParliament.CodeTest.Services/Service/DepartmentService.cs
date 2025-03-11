using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Interface;

namespace UKParliament.CodeTest.Services.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                return await _departmentRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                //LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error occurred retrieving departments");
            }
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            try
            {
                return await _departmentRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                //LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error occurred retrieving department");
            }
        }
    }
}