using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.RolDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")] //pongo aqui para que todos sean necesarios autorización
    public class RolController : ControllerBase
    {
        private readonly IServiceGeneric<RolUpdateDto, RolCreateDto> _service;
        public RolController(IServiceGeneric<RolUpdateDto, RolCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRoles()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("{id}"), Name = "GetRolbyId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolById(int id) //get para traer con id
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("nombre/{name}"), Name = "GetRolbyName")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolByName(String name) //get para traer con nombre
        {
            var result = await _service.GetByName(name);
            return Utils.ControllerHelper(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateRol([FromBody] RolCreateDto rolCreateDto)
        {
            var result = await _service.Create(rolCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> UpdateRol([FromBody] RolUpdateDto rolUpdateDto)
        {
            var result = await _service.Update(rolUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeleteRol")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteRol(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialRol")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdatePartialRol(int id, JsonPatchDocument<RolUpdateDto> rolUpdateDto)
        {
            var result = await _service.UpdatePartial(id, rolUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}
