using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Application.Conversions.Interfaces
{
    public interface IDepartmentConversion
    {
        Department ToEntity(DepartmentViewModel departmentViewModel);

        DepartmentViewModel ToViewModel(Department department);

        IEnumerable<DepartmentViewModel> ToViewModelList(IEnumerable<Department> departments);
    }
}