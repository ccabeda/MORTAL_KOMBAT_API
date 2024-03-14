using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClanController : ControllerBase
    {
        private readonly IServiceGeneric<ClanUpdateDto, ClanCreateDto> _service;
        public ClanController(IServiceGeneric<ClanUpdateDto, ClanCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClanes()
        {
            var result = await _service.GetAll();
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("{id}"), Name = "GetClanbyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClanById(int id) //get para traer con id
        {
            var result = await _service.GetById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("nombre/{name}"), Name = "GetClanbyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetClanByName(String name) //get para traer con nombre
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
        public async Task<ActionResult<APIResponse>> CreateClan([FromBody] ClanCreateDto clanCreateDto)
        {
            var result = await _service.Create(clanCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> UpdateClan([FromBody] ClanUpdateDto clanUpdateDto)
        {
            var result = await _service.Update(clanUpdateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "DeleteClan")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteClan(int id)
        {
            var result = await _service.Delete(id);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialClan")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdatePartialClan(int id, JsonPatchDocument<ClanUpdateDto> clanUpdateDto)
        {
            var result = await _service.UpdatePartial(id, clanUpdateDto);
            return Utils.ControllerHelper(result);
        }
    }
}
