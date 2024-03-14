using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArmaController : ControllerBase
    {
        private readonly IServiceGeneric<ArmaUpdateDto, ArmaCreateDto> _service;
        public ArmaController(IServiceGeneric<ArmaUpdateDto, ArmaCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmas()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("{id}"), Name = "GetArmabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmaById(int id) //get para traer con id
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("nombre/{name}"), Name = "GetArmabyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmaByName(String name) //get para traer con nombre
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
        public async Task<ActionResult<APIResponse>> CreateArma([FromBody] ArmaCreateDto armaCreateDto)
        {
            var result = await _service.Create(armaCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> UpdateArma([FromBody] ArmaUpdateDto armaUpdateDto)
        {
            var result = await _service.Update(armaUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeleteArma")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteArma(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialArma")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialArma(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            var result = await _service.UpdatePartial(id, armaUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}
