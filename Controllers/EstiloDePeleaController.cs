﻿using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> PutEstiloDePelea(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
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
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

