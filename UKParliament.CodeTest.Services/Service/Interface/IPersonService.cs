using System.Linq.Expressions;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Services.Service.Interface;

public interface IPersonService
{
    Task<Response> AddPersonAsync(PersonViewModel person);

    Task<Response> DeletePersonAsync(int id);

    Task<IEnumerable<PersonViewModel>> GetAllPeopleAsync();

    Task<Person> GetByAsync(Expression<Func<Person, bool>> predicate);

    Task<PersonViewModel> GetPersonByIdAsync(int id);

    Task<Response> UpdatePersonAsync(PersonViewModel entity);
}