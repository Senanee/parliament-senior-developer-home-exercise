using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Application.Conversions.Interfaces
{
    public interface IPersonConversion
    {
        Person ToEntity(PersonViewModel personViewModel);
        PersonViewModel ToViewModel(Person person);
        IEnumerable<PersonViewModel> ToViewModelList(IEnumerable<Person> people);
    }
}
