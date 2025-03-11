using System.Linq.Expressions;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.Logs;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Service.Interface;

namespace UKParliament.CodeTest.Services.Service;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IPersonConversion _personConversion;

    public PersonService(IPersonRepository personRepository,IPersonConversion personConversion)
    {
        _personRepository = personRepository;
        _personConversion = personConversion;
    }

    public async Task<IEnumerable<PersonViewModel>> GetAllPeopleAsync()
    {
        try
        {
            var people = await _personRepository.GetAllAsync();
            return _personConversion.ToViewModelList(people);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new InvalidOperationException("Error occurred retrieving people");
        }
    }

    public async Task<PersonViewModel> GetPersonByIdAsync(int id)
    {
        try
        {
            var person = await _personRepository.GetByIdAsync(id);
            return _personConversion.ToViewModel(person);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retrieving entity");
        }
    }

    public async Task<Response> AddPersonAsync(PersonViewModel person)
    {
        try
        {
            var personEntity = _personConversion.ToEntity(person);
            var currentEntity = await _personRepository.AddAsync(personEntity);

            if (currentEntity is not null && currentEntity.Id > 0)
                return new Response(true, $"{person.FirstName} {person.LastName} added to database successfully.");
            else
                return new Response(false, $"Error occurred while adding {person.FirstName} {person.LastName}");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            return new Response(false, "Error occurred adding new entity");
        }
    }

    public async Task<Response> UpdatePersonAsync(PersonViewModel personViewModel)
    {
        try
        {
            var personEntity = _personConversion.ToEntity(personViewModel);
            var person = await _personRepository.GetByIdAsync(personViewModel.Id);

            if (person is null)
                return new Response(false, $"{personViewModel.FirstName} {personViewModel.LastName} not found");

            await _personRepository.UpdateAsync(person);

            return new Response(true, $"{personViewModel.FirstName} {personViewModel.LastName} is updated successfully");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            return new Response(false, "Error occurred updating person");
        }
    }

    public async Task<Response> DeletePersonAsync(int id)
    {
        try
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person is null)
                return new Response(false, $"Person not found");

            await _personRepository.DeleteAsync(id);

            return new Response(true, $"{person.FirstName} {person.LastName} is deleted successfully");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            return new Response(false, "Error occurred Deleting entity");
        }
    }

    public async Task<Person> GetByAsync(Expression<Func<Person, bool>> predicate)
    {
        try
        {
            return await _personRepository.GetByAsync(predicate);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retrieving entity");
        }
    }
}