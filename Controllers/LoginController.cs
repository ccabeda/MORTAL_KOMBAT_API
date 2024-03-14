using API_MortalKombat.Models;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //documentamos el estado 400
        public async Task<ActionResult<APIResponse>> LoginUsuario(Login userAndPass) //metodo para logearse
        {
            var result = await _service.LoginUsuario(userAndPass); //creo el token de logeo o null si falla
            return Utils.ControllerHelper(result);
        }
    }
}
