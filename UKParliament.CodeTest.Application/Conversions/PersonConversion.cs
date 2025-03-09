using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Application.Conversions
{
    public static class PersonConversion
    {
        public static Person ToEntity(PersonViewModel person) => new()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId
        };

        public static (PersonViewModel?, IEnumerable<PersonViewModel>?) FromEntity(Person person, IEnumerable<Person>? people)
        {
            if (person is not null || people is null)
            {
                var singleProduct = new PersonViewModel(person!.Id, person.FirstName!, person.LastName!, person.DateOfBirth!, person.DepartmentId!);
                return (singleProduct, null);
            }

            if (people is not null || person is null)
            {
                var _people = people!.Select(person =>
                    new PersonViewModel(person!.Id, person.FirstName!, person.LastName, person.DateOfBirth, person!.DepartmentId));
                return (null, _people);
            }

            return (null, null);
        }
    }
}
