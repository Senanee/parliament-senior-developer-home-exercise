using System.Linq.Expressions;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task<Person> AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int id);
        Task<Person> GetByAsync(Expression<Func<Person, bool>> predicate);
    }
}
