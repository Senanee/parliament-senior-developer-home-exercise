using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PersonManagerContext _context;

        public DepartmentRepository(PersonManagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var departments = await _context.Departments.AsNoTracking().ToListAsync();
            return departments is not null ? departments : null!;
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department is not null ? department : null!;
        }
    }
}