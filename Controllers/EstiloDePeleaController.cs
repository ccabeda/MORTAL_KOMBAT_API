using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstiloDePeleaController : ControllerBase
    {
        private readonly IServiceEstiloDePelea _service;
        public EstiloDePeleaController(IServiceEstiloDePelea service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstilosDePelea()
        {
            var result = await _service.GetEstilosDePelea();
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetEstiloDePeleabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstiloDePeleaById(int id) //get para traer con id
        {
            var result = await _service.GetEstiloDePeleaById(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpGet(("nombre/{name}"), Name = "GetEstiloDePeleabyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstiloDePeleaByName(String name) //get para traer con nombre
        {
            var result = await _service.GetEstiloDePeleaByName(name);
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
        public async Task<ActionResult<APIResponse>> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            var result = await _service.CreateEstiloDePelea(estiloCreateDto);
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

        [HttpPut(("{id}"), Name = "PutEstiloDePeleabyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> PutEstiloDePelea(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            var result = await _service.UpdateEstiloDePelea(id, estiloUpdateDto);
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

        [HttpDelete(("{id}"), Name = "DeleteEstiloDePelea")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteEstiloDePelea(int id)
        {
            var result = await _service.DeleteEstiloDePelea(id);
            switch (result.statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(result);
                default:
                    return NotFound(result);
            }
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialEstiloDePelea")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartiaEstiloDePelea(int id, JsonPatchDocument<EstiloDePeleaUpdateDto> estiloDePeleaUpdateDto) 
        {
            var result = await _service.UpdatePartialEstiloDePelea(id, estiloDePeleaUpdateDto);
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

