using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repositories.Interfaces;

namespace UKParliament.CodeTest.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonManagerContext _context;

        public PersonRepository(PersonManagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var people = await _context.People.AsNoTracking().ToListAsync();
            return people is not null ? people : null!;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var person = await _context.People.FindAsync(id);
            return person is not null ? person : null!;
        }

        public async Task<Person> AddAsync(Person person)
        {
            var currentEntity = await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            return currentEntity.Entity;
        }

        public async Task UpdateAsync(Person person)
        {
            var existingPerson = await _context.People.FindAsync(person.Id);
            if (existingPerson == null)
            {
                throw new DbUpdateConcurrencyException("Attempted to update an entity that does not exist in the store.");
            }

            _context.Entry(existingPerson).CurrentValues.SetValues(person);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Person> GetByAsync(Expression<Func<Person, bool>> predicate)
        {
            var people = await _context.People.Where(predicate).FirstOrDefaultAsync();
            return people is not null ? people : null!;
        }
    }
}