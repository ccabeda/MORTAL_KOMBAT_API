﻿using API_MortalKombat.Models;
using API_MortalKombat.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IServiceLogin _service;
        public LoginController(IServiceLogin service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)] //documentamos el estado 200
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentamos el estado 400
        public async Task<ActionResult<APIResponse>> LoginUsuario(Login usuario) //metodo para logearse
        {
            var result = await _service.LoginUsuario(usuario); //creo el token de logeo o null si falla
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
