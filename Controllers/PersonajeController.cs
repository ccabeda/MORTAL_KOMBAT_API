using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;
using MortalKombat_API.Data;
using MortalKombat_API.Models;
using MortalKombat_API.Models.DTOs.PersonajeDTO;
using System.Net;

namespace MortalKombat_API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class PersonajeController : ControllerBase
    {
        private readonly IServicePersonaje _service;
        public PersonajeController(IServicePersonaje service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajes()
        {
            var result = await _service.GetPersonajes();
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        } 

        [HttpGet(("{id}"), Name = "GetPersonajebyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajeById(int id) //get para traer con id
        {
            var result = await _service.GetPersonajeById(id);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else if (result.statusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }
            else 
            {
                return NotFound(result);
            }
        }  

        [HttpGet(("nombre/{name}"), Name = "GetPersonajebyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajeByName(String name) //get para traer con nombre
        {
            var result = await _service.GetPersonajeByName(name);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else if (result.statusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }
            else
            {
                return NotFound(result);
            }
        } 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreatePersonaje ([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            var result = await _service.CreatePersonaje(personajeCreateDto);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else if (result.statusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == HttpStatusCode.Created)
            {
                return Ok(result);
            }
            else if (result.statusCode == HttpStatusCode.Conflict)
            {
                return Conflict(result);
            }
            else
            {
                return NotFound(result);
            }

        }

        [HttpPut(("{id}"), Name = "PutPersonabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> PutPersonaje (int id, [FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            var result = await _service.PutPersonaje(id, personajeUpdateDto);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpDelete(("{id}"), Name = "DeletePersonaje")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePersonaje(int id)
        {
            var result = await _service.DeletePersonaje(id);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

    }
}
