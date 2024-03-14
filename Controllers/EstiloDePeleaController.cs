using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstiloDePeleaController : ControllerBase
    {
        private readonly IServiceGeneric<EstiloDePeleaUpdateDto, EstiloDePeleaCreateDto> _service;
        public EstiloDePeleaController(IServiceGeneric<EstiloDePeleaUpdateDto, EstiloDePeleaCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstilosDePelea()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("{id}"), Name = "GetEstiloDePeleabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstiloDePeleaById(int id) //get para traer con id
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("nombre/{name}"), Name = "GetEstiloDePeleabyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstiloDePeleaByName(String name) //get para traer con nombre
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
        public async Task<ActionResult<APIResponse>> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            var result = await _service.Create(estiloCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> UpdateEstiloDePelea([FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            var result = await _service.Update(estiloUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeleteEstiloDePelea")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteEstiloDePelea(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialEstiloDePelea")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdatePartiaEstiloDePelea(int id, JsonPatchDocument<EstiloDePeleaUpdateDto> estiloDePeleaUpdateDto) 
        {
            var result = await _service.UpdatePartial(id, estiloDePeleaUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}

