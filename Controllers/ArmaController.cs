﻿using API_MortalKombat.Models;
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
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet(("{id}"), Name = "GetArmabyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmaById(int id) //get para traer con id
        {
            var result = await _service.GetArmaById(id);
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

        [HttpGet(("nombre/{name}"), Name = "GetArmabyName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetArmaByName(String name) //get para traer con nombre
        {
            var result = await _service.GetArmaByName(name);
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
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateArma([FromBody] ArmaCreateDto armaCreateDto)
        {
            var result = await _service.CreateArma(armaCreateDto);
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

        [HttpPut(("{id}"), Name = "PutArmabyId")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> PutArma(int id, [FromBody] ArmaUpdateDto armaUpdateDto)
        {
            var result = await _service.UpdateArma(id, armaUpdateDto);
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
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
            if (result.statusCode == HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPatch(("{id}"), Name = "UpdatePartialArma")]
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartiaArma(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            var result = await _service.UpdatePartialArma(id, armaUpdateDto);
            if (result.statusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
