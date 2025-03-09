using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Interface;

namespace UKParliament.CodeTest.Services.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly PersonManagerContext _context;

        public DepartmentService(PersonManagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                var departments = await _context.Departments.AsNoTracking().ToListAsync();
                return departments is not null ? departments : null!;
            }
            catch (Exception ex)
            {
                //LogException.LogExceptions(ex);

                throw new InvalidOperationException("Error occurred retriving departments");
            }
        }


    }
}
