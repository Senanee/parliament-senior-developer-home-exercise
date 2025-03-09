using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Interface;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PersonController : ControllerBase
{
    private readonly IPersonService personInterface;

    public PersonController(IPersonService personService)
    {
        personInterface = personService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetPeople()
    {
        var people = await personInterface.GetAllPeopleAsync();
        if (!people.Any())
            return NotFound("No persons detected in the database");

        var (_, list) = PersonConversion.FromEntity(null!, people);
        return list!.Any() ? Ok(list) : NotFound("No person found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonViewModel>> GetPerson(int id)
    {
        var person = await personInterface.GetPersonByIdAsync(id);
        if (person == null)
        {
            return NotFound("No person found");
        }
        var (_person, _) = PersonConversion.FromEntity(person, null!);
        return _person is not null ? Ok(_person) : NotFound("No person found");
    }

    [HttpPost]
    public async Task<ActionResult<Response>> AddPerson(PersonViewModel person)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var getEntity = PersonConversion.ToEntity(person);
        var response = await personInterface.AddPersonAsync(getEntity);
        return response.Flag is true ? CreatedAtAction(nameof(AddPerson), response) : BadRequest(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response>> UpdatePerson(PersonViewModel person)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var getEntity = PersonConversion.ToEntity(person);
        var response = await personInterface.UpdatePersonAsync(getEntity);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var response = await personInterface.DeletePersonAsync(id);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }
}