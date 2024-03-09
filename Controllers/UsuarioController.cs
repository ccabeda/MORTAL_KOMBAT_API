using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IServiceUsuario _service;
        public UsuarioController(IServiceUsuario service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarios()
        {
            var result = await _service.GetUsuarios();
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetUsuariobyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioById(int id) //get para traer con id
        {
            var result = await _service.GetUsuarioById(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("nombreDeUsuario/{name}"), Name = "GetUsuariobyName")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioByName(String name) //get para traer con nombre
        {
            var result = await _service.GetUsuarioByName(name);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            var result = await _service.CreateUsuario(usuarioCreateDto);
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

        [HttpPut(("{id}"), Name = "PutUsuariobyId")]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> PutUsuario(int id, [FromBody] UsuarioUpdateDto usuarioUpdateDto)
        {
            var result = await _service.UpdateUsuario(id, usuarioUpdateDto);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                case HttpStatusCode.BadRequest:
                    return BadRequest(result);
                case HttpStatusCode.Conflict:
                    return Conflict(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpDelete(("{id}"), Name = "DeleteUsuario")]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteUsuario(int id)
        {
            var result = await _service.DeleteUsuario(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialUsuario")]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartialUsuario(int id, JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto)
        {
            var result = await _service.UpdatePartialUsuario(id, usuarioUpdateDto);
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
