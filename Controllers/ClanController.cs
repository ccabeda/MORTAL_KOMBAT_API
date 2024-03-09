using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClanController : ControllerBase
    {
        private readonly IServiceClan _service;
        public ClanController(IServiceClan service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClanes()
        {
            var result = await _service.GetClanes();
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetClanbyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClanById(int id) //get para traer con id
        {
            var result = await _service.GetClanById(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("nombre/{name}"), Name = "GetClanbyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClanByName(String name) //get para traer con nombre
        {
            var result = await _service.GetClanByName(name);
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
        public async Task<ActionResult<APIResponse>> CreateClan([FromBody] ClanCreateDto clanCreateDto)
        {
            var result = await _service.CreateClan(clanCreateDto);
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

        [HttpPut(("{id}"), Name = "PutClanbyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> PutClan(int id, [FromBody] ClanUpdateDto clanUpdateDto)
        {
            var result = await _service.UpdateClan(id, clanUpdateDto);
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

        [HttpDelete(("{id}"), Name = "DeleteClan")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteClan(int id)
        {
            var result = await _service.DeleteClan(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialClan")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartialClan(int id, JsonPatchDocument<ClanUpdateDto> clanUpdateDto)
        {
            var result = await _service.UpdatePartialClan(id, clanUpdateDto);
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
