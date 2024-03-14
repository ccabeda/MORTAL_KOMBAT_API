using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        } 

        [HttpGet(("{id}"), Name = "GetPersonajebyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajeById(int id) //get para traer con id
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }  

        [HttpGet(("nombre/{name}"), Name = "GetPersonajebyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPersonajeByName(String name) //get para traer con nombre
        {
            var result = await _service.GetByName(name);
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
            var result = await _service.Create(personajeCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> UpdatePersonaje([FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            var result = await _service.Update(personajeUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeletePersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePersonaje(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{idPersonaje}/AddWeapon/{idArma}"), Name = "AddWeaponToPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> AddWeaponToPersonaje(int idPersonaje, int idArma)
        {
            var result = await _service.AddWeapon(idPersonaje, idArma);
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
            var result = await _service.RemoveWeapon(idPersonaje, idArma);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{idPersonaje}/AddStyle/{idEstilDePelea}"), Name = "AddStyleToPersonaje")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> AddStyleToPersonaje(int idPersonaje, int idEstilDePelea)
        {
            var result = await _service.AddStyle(idPersonaje, idEstilDePelea);
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
            var result = await _service.RemoveStyle(idPersonaje, idEstilDePelea);
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
            var result = await _service.UpdatePartial(id, personajeUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}
