using System.Linq.Expressions;
using UKParliament.CodeTest.Application.Logs;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Services.Interface;

namespace UKParliament.CodeTest.Services.Service;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IValidationService _validationService;

    public PersonService(IPersonRepository personRepository, IValidationService validationService)
    {
        _personRepository = personRepository;
        _validationService = validationService;
    }

    public async Task<IEnumerable<Person>> GetAllPeopleAsync()
    {
        try
        {
            return await _personRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retrieving people");
        }
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        try
        {
            return await _personRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retrieving entity");
        }
    }

    public async Task<Response> AddPersonAsync(Person person)
    {
        try
        {
            var validationResults = _validationService.Validate(person);
            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                return new Response { Flag = false, message = errorMessage };
            }

            var currentEntity = await _personRepository.AddAsync(person);

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

    public async Task<Response> UpdatePersonAsync(Person entity)
    {
        try
        {
            var validationResults = _validationService.Validate(entity);
            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                return new Response { Flag = false, message = errorMessage };
            }

            var person = await _personRepository.GetByIdAsync(entity.Id);

            if (person is null)
                return new Response(false, $"{entity.FirstName} {entity.LastName} not found");

            await _personRepository.UpdateAsync(entity);

            return new Response(true, $"{entity.FirstName} {entity.LastName} is updated successfully");
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