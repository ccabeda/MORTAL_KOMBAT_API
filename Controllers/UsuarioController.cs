using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;

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
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("{id}"), Name = "GetUsuariobyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioById(int id) //get para traer con id
        {
            var result = await _service.GetUsuarioById(id);
            return Utils.ControllerHelper(result);
        }

        [HttpGet(("nombreDeUsuario/{name}"), Name = "GetUsuariobyName")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuarioByName(String name) //get para traer con nombre
        {
            var result = await _service.GetUsuarioByName(name);
            return Utils.ControllerHelper(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateMyUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            var result = await _service.CreateMyUsuario(usuarioCreateDto);
            return Utils.ControllerHelper(result);
        }

        [HttpPut(("{username}/{password}"), Name = "PutMyUsuario")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> PutMyUsuario([FromBody] UsuarioUpdateDto usuarioUpdateDto, string username, string password)
        {
            var result = await _service.UpdateMyUsuario(usuarioUpdateDto,username, password);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{id}"), Name = "ADMIN_DeleteUsuario")]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ADMIN_DeleteUsuario(int id)
        {
            var result = await _service.ADMIN_DeleteUsuario(id);
            return Utils.ControllerHelper(result);
        }

        [HttpDelete(("{username}/{password}"), Name = "DeleteMyUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteMyUsuario(string username, string password)
        {
            var result = await _service.DeleteMyUsuario(username, password);
            return Utils.ControllerHelper(result);
        }

        [HttpPatch(("{username}/{password}"), Name = "UpdatePartialMyUsuario")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdatePartialMyUsuario(JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto, string username, string password)
        {
            var result = await _service.UpdatePartialMyUsuario(usuarioUpdateDto, username, password);
            return Utils.ControllerHelper(result);
        }
    }
}
