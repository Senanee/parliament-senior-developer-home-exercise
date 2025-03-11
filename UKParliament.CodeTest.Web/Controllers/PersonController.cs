using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Application.Responses;
using UKParliament.CodeTest.Application.ViewModels;
using UKParliament.CodeTest.Services.Service.Interface;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IValidationService _validationService;

    public PersonController(IPersonService personService, IValidationService validationService)
    {
        _personService = personService;
        _validationService = validationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetPeople()
    {
        var people = await _personService.GetAllPeopleAsync();
        return people!.Any() ? Ok(people) : NotFound("No person found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonViewModel>> GetPerson(int id)
    {
        var person = await _personService.GetPersonByIdAsync(id);

        if (person == null || !(person.Id > 0))
            return NotFound("No person found");

        return person is not null ? Ok(person) : NotFound("No person found");
    }

    [HttpPost]
    public async Task<ActionResult<Response>> AddPerson(PersonViewModel person)
    {
        var validationResults = _validationService.Validate(person);
        if (validationResults.Count > 0)
        {
            var errorMessage = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            return BadRequest( new Response { Flag = false, message = errorMessage });
        }

        var response = await _personService.AddPersonAsync(person);

        if (!response.Flag)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, response);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response>> UpdatePerson(PersonViewModel person)
    {
        var validationResults = _validationService.Validate(person);

        if (validationResults.Count > 0)
        {
            var errorMessage = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
            return BadRequest(new Response { Flag = false, message = errorMessage });
        }

        var response = await _personService.UpdatePersonAsync(person);
        if (!response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response>> DeletePerson(int id)
    {
        var response = await _personService.DeletePersonAsync(id);
        return response.Flag is true ? Ok(response) : BadRequest(response);
    }
}