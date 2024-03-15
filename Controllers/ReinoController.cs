using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReinoController : ControllerBase
    {
        private readonly IServiceGeneric<ReinoUpdateDto, ReinoCreateDto> _service;
        public ReinoController(IServiceGeneric<ReinoUpdateDto, ReinoCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetReinos()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("{id}"), Name = "GetReinobyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetReinoById(int id) //get para traer con id
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("nombre/{name}"), Name = "GetReinobyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetReinoByName(String name) //get para traer con nombre
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateReino([FromBody] ReinoCreateDto reinoCreateDto)
        {
            var result = await _service.Create(reinoCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateReino([FromBody] ReinoUpdateDto reinoUpdateDto)
        {
            var result = await _service.Update(reinoUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeleteReino")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteReino(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialReino")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdatePartialReino(int id, JsonPatchDocument<ReinoUpdateDto> reinoUpdateDto)
        {
            var result = await _service.UpdatePartial(id, reinoUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}
