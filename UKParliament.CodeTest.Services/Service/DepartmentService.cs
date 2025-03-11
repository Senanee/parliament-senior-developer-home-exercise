using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.Logs;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Service.Interface;

namespace UKParliament.CodeTest.Services.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentConversion _departmentConversion;

        public DepartmentService(IDepartmentRepository departmentRepository, IDepartmentConversion departmentConversion)
        {
            _departmentRepository = departmentRepository;
            _departmentConversion = departmentConversion;
        }

        public async Task<IEnumerable<DepartmentViewModel>> GetAllDepartmentsAsync()
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                return _departmentConversion.ToViewModelList(departments);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error occurred retrieving departments");
            }
        }

        public async Task<DepartmentViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                return _departmentConversion.ToViewModel(department);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error occurred retrieving departments");
            }
        }
    }
}