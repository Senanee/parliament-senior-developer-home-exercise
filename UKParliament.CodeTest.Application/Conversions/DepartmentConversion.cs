using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Application.Conversions
{
    public class DepartmentConversion : IDepartmentConversion
    {
        public Department ToEntity(DepartmentViewModel departmentViewModel)
        {
            if (departmentViewModel == null) throw new ArgumentNullException(nameof(departmentViewModel));

            return new Department
            {
                Id = departmentViewModel.Id,
                Name = departmentViewModel.Name,
            };
        }

        public DepartmentViewModel ToViewModel(Department department)
        {
            if (department == null) throw new ArgumentNullException(nameof(department));

            return new DepartmentViewModel(department.Id, department.Name);
        }

        public IEnumerable<DepartmentViewModel> ToViewModelList(IEnumerable<Department> departments)
        {
            if (departments == null) throw new ArgumentNullException(nameof(departments));

            return departments.Select(d => new DepartmentViewModel(d.Id, d.Name)).ToList();
        }
    }
}
