using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Service.IService;
using API_MortalKombat.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Controllers
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
            return Utils.ControllerHelper(result);
        } 

        [HttpGet(("{id}"), Name = "GetPersonajebyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajeById(int id) //get para traer con id
        {
            var result = await _service.GetPersonajeById(id);
            return Utils.ControllerHelper(result);
        }  

        [HttpGet(("nombre/{name}"), Name = "GetPersonajebyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajeByName(String name) //get para traer con nombre
        {
            var result = await _service.GetPersonajeByName(name);
            return Utils.ControllerHelper(result);
        } 

        [HttpPost]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreatePersonaje ([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            var result = await _service.CreatePersonaje(personajeCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{id}"), Name = "PutPersonabyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> UpdatePersonaje (int id, [FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            var result = await _service.UpdatePersonaje(id, personajeUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeletePersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePersonaje(int id)
        {
            var result = await _service.DeletePersonaje(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{idPersonaje}/AddWeapon/{idArma}"), Name = "AddWeaponToPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> AddWeaponToPersonaje(int idPersonaje, int idArma)
        {
            var result = await _service.AddWeaponToPersonaje(idPersonaje, idArma);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{idPersonaje}/RemoveWeapon/{idArma}"), Name = "RemoveWeaponToPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> RemoveWeaponToPersonaje(int idPersonaje, int idArma)
        {
            var result = await _service.RemoveWeaponToPersonaje(idPersonaje, idArma);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{idPersonaje}/AddStyle/{idEstilDePelea}"), Name = "AddStyleToPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> AddStyleToPersonaje(int idPersonaje, int idEstilDePelea)
        {
            var result = await _service.AddStyleToPersonaje(idPersonaje, idEstilDePelea);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{idPersonaje}/RemoveStyle/{idEstilDePelea}"), Name = "RemoveStyleToPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> RemoveStyleToPersonaje(int idPersonaje, int idEstilDePelea)
        {
            var result = await _service.RemoveStyleToPersonaje(idPersonaje, idEstilDePelea);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdatePartialPersonaje(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto)
        {
            var result = await _service.UpdatePartialPersonaje(id, personajeUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}
