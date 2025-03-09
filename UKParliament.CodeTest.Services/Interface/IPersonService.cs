using System.Linq.Expressions;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Interface;

public interface IPersonService
{
    Task<Response> AddPersonAsync(Person person);
    Task<Response> DeletePersonAsync(int id);
    Task<Person> FindByIdAsync(int id);
    Task<IEnumerable<Person>> GetAllPeopleAsync();
    Task<Person> GetByAsync(Expression<Func<Person, bool>> predicate);
    Task<Person> GetPersonByIdAsync(int id);
    Task<Response> UpdatePersonAsync(Person entity);
}