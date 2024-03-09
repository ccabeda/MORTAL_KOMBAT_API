using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArmaController : ControllerBase
    {
        private readonly IServiceArma _service;
        public ArmaController(IServiceArma service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmas()
        {
            var result = await _service.GetArmas();
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetArmabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmaById(int id) //get para traer con id
        {
            var result = await _service.GetArmaById(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("nombre/{name}"), Name = "GetArmabyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmaByName(String name) //get para traer con nombre
        {
            var result = await _service.GetArmaByName(name);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPost]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateArma([FromBody] ArmaCreateDto armaCreateDto)
        {
            var result = await _service.CreateArma(armaCreateDto);
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

        [HttpPut(("{id}"), Name = "PutArmabyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> PutArma(int id, [FromBody] ArmaUpdateDto armaUpdateDto)
        {
            var result = await _service.UpdateArma(id, armaUpdateDto);
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

        [HttpDelete(("{id}"), Name = "DeleteArma")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteArma(int id)
        {
            var result = await _service.DeleteArma(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialArma")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartialArma(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            var result = await _service.UpdatePartialArma(id, armaUpdateDto);
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
