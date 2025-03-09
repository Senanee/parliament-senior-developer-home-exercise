using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Application.Conversions
{
    public static class DepartmentConversion
    {
        public static Department ToEntity(DepartmentViewModel department) => new()
        {
            Id = department.Id,
            Name = department.Name,
        };

        public static (DepartmentViewModel?, IEnumerable<DepartmentViewModel>?) FromEntity(Department department, IEnumerable<Department>? departments)
        {
            if (department is not null || departments is null)
            {
                var singleProduct = new DepartmentViewModel(department!.Id, department.Name!);
                return (singleProduct, null);
            }

            if (departments is not null || department is null)
            {
                var _departments = departments!.Select(d =>
                    new DepartmentViewModel(d!.Id, d.Name!)).ToList();
                return (null, _departments);
            }

            return (null, null);
        }
    }
}
