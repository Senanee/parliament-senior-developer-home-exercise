using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Conversions.Interfaces;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Interface;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IPersonConversion _personConversion;
    private readonly IValidationService _validationService;

    public PersonController(IPersonService personService, IPersonConversion personConversion, IValidationService validationService)
    {
        _personService = personService;
        _personConversion = personConversion;
        _validationService = validationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetPeople()
    {
        var people = await _personService.GetAllPeopleAsync();

        if (!people.Any())
            return NotFound("No persons detected in the database");

        var list = _personConversion.ToViewModelList(people);
        return list!.Any() ? Ok(list) : NotFound("No person found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonViewModel>> GetPerson(int id)
    {
        var person = await _personService.GetPersonByIdAsync(id);

        if (person == null || !(person.Id > 0))
            return NotFound("No person found");

        var _person = _personConversion.ToViewModel(person);
        return _person is not null ? Ok(_person) : NotFound("No person found");
    }

    [HttpPost]
    public async Task<ActionResult<Response>> AddPerson(PersonViewModel person)
    {
        var personEntity = _personConversion.ToEntity(person);
        var response = await _personService.AddPersonAsync(personEntity);

        if (!response.Flag)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetPerson), new { id = personEntity.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response>> UpdatePerson(PersonViewModel person)
    {
        var personEntity = _personConversion.ToEntity(person);
        var response = await _personService.UpdatePersonAsync(personEntity);
        if (!response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var response = await _personService.DeletePersonAsync(id);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }
}