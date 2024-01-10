using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;

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
        public async Task<ActionResult<APIResponse>> GetEstilosDePeleas()
        {
            var result = await _service.GetEstilosDePeleas();
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetEstiloDePeleabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstiloDePeleaById(int id) //get para traer con id
        {
            var result = await _service.GetEstiloDePeleaById(id);
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

        [HttpGet(("nombre/{name}"), Name = "GetEstiloDePeleabyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstiloDePeleaByName(String name) //get para traer con nombre
        {
            var result = await _service.GetEstiloDePeleaByName(name);
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            var result = await _service.CreateEstiloDePelea(estiloCreateDto);
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

        [HttpPut(("{id}"), Name = "PutEstiloDePeleabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> PutArma(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            var result = await _service.UpdateEstiloDePelea(id, estiloUpdateDto);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpDelete(("{id}"), Name = "DeleteEstiloDePelea")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteEstiloDePelea(int id)
        {
            var result = await _service.DeleteEstiloDePelea(id);
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

