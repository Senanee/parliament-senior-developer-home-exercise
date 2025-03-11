using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Application.Conversions
{
    public class PersonConversion : IPersonConversion
    {
        public Person ToEntity(PersonViewModel personViewModel)
        {
            if (personViewModel == null) throw new ArgumentNullException(nameof(personViewModel));

            return new Person
            {
                Id = personViewModel.Id,
                FirstName = personViewModel.FirstName,
                LastName = personViewModel.LastName,
                DateOfBirth = personViewModel.DateOfBirth,
                DepartmentId = personViewModel.DepartmentId,
                Email = personViewModel.Email
            };
        }

        public PersonViewModel ToViewModel(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));

            return new PersonViewModel()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                DepartmentId = person.DepartmentId,
                Email = person.Email
            };
        }

        public IEnumerable<PersonViewModel> ToViewModelList(IEnumerable<Person> people)
        {
            if (people == null) throw new ArgumentNullException(nameof(people));

            return people.Select(person => new PersonViewModel()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                DepartmentId = person.DepartmentId,
                Email = person.Email
            });
        }
    }
}