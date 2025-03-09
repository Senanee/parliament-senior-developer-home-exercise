using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Interface;

namespace UKParliament.CodeTest.Services.Service;

public class PersonService : IPersonService
{
    private readonly PersonManagerContext _context;

    public PersonService(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetAllPeopleAsync()
    {
        try
        {
            var people = await _context.People.Include(p => p.Department).AsNoTracking().ToListAsync();
            return people is not null ? people : null!;
        }
        catch (Exception ex)
        {
            //LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retriving people");
        }
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        try
        {
            var person = await _context.People.Include(p => p.Department).FirstOrDefaultAsync(p => p.Id == id); ;

            return person is not null ? person : null!;
        }
        catch (Exception ex)
        {
            //LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retriving entity");
        }
    }

    public async Task<Response> AddPersonAsync(Person person)
    {
        try
        {
            var currentEntity = _context.People.Add(person).Entity;
            await _context.SaveChangesAsync();

            if (currentEntity is not null && currentEntity.Id > 0)
                return new Response(true, $"{person.FirstName} {person.LastName} added to database successfully.");
            else
                return new Response(false, $"Error occurred while adding {person.FirstName} {person.LastName}");
        }
        catch (Exception ex)
        {
            //LogException.LogExceptions(ex);

            return new Response(false, "Error occurred adding new entity");
        }
    }

    public async Task<Response> UpdatePersonAsync(Person entity)
    {
        try
        {
            var person = await FindByIdAsync(entity.Id);

            if (person is null)
                return new Response(false, $"{entity.FirstName} {entity.LastName} not found");

            _context.Entry(person).State = EntityState.Detached;
            _context.People.Update(entity);
            await _context.SaveChangesAsync();

            return new Response(true, $"{entity.FirstName} {entity.LastName} is updated successfully");
        }
        catch (Exception ex)
        {
            //LogException.LogExceptions(ex);

            return new Response(false, "Error occurred updating person");
        }
    }

    public async Task<Response> DeletePersonAsync(int id)
    {
         try
            {
                var person = await FindByIdAsync(id);

                if (person is null)
                    return new Response(false, $"Person not found");

                _context.People.Remove(person);
                await _context.SaveChangesAsync();

                return new Response(true, $"{person.FirstName} {person.LastName} is deleted successfully");
            }
            catch (Exception ex)
            {
                //LogException.LogExceptions(ex);

                return new Response(false, "Error occurred Deleting entity");
            }
    }

    public async Task<Person> GetByAsync(Expression<Func<Person, bool>> predicate)
    {
        try
        {
            var people = await _context.People.Where(predicate).FirstOrDefaultAsync();
            return people is not null ? people : null!;
        }
        catch (Exception ex)
        {
            //LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retriving entity");
        }
    }

    public async Task<Person> FindByIdAsync(int id)
    {
        try
        {
            var person = await _context.People.FindAsync(id);

            return person is not null ? person : null!;
        }
        catch (Exception ex)
        {
            //LogException.LogExceptions(ex);

            throw new InvalidOperationException("Error occurred retriving entity");
        }
    }
}
