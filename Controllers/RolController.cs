using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.RolDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")] //pongo aqui para que todos sean necesarios autorización
    public class RolController : ControllerBase
    {
        private readonly IServiceRol _service;
        public RolController(IServiceRol service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRoles()
        {
            var result = await _service.GetRoles();
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetRolbyId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolById(int id) //get para traer con id
        {
            var result = await _service.GetRolById(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("nombre/{name}"), Name = "GetRolbyName")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRolByName(String name) //get para traer con nombre
        {
            var result = await _service.GetRolByName(name);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateRol([FromBody] RolCreateDto rolCreateDto)
        {
            var result = await _service.CreateRol(rolCreateDto);
            switch (result.statusCode)
            {
                case HttpStatusCode.Created:
                    return Ok(result);
                case HttpStatusCode.Conflict:
                    return Conflict(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPut(("{id}"), Name = "PutRolbyId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> PutRol(int id, [FromBody] RolUpdateDto rolUpdateDto)
        {
            var result = await _service.UpdateRol(id, rolUpdateDto);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return Conflict(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpDelete(("{id}"), Name = "DeleteRol")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteRol(int id)
        {
            var result = await _service.DeleteRol(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialRol")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartialRol(int id, JsonPatchDocument<RolUpdateDto> rolUpdateDto)
        {
            var result = await _service.UpdatePartialRol(id, rolUpdateDto);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }
    }
}
