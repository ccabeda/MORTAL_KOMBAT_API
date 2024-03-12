using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace API_MortalKombat.Service
{
    public class ServiceLogin : IServiceLogin
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly ILogger _logger;
        private readonly APIResponse _apiresponse;
        private readonly IConfiguration _config;
        public ServiceLogin(IRepositoryUsuario repositoryUsuario, IConfiguration config, ILogger<ServiceLogin> logger, APIResponse response)
        {
            _repositoryUsuario = repositoryUsuario;
            _config = config;
            _logger = logger;
            _apiresponse = response;
        }

        public string GenerarTokendeLogin(Usuario user)
        {
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)); //configurar cosas de creación de token JWT 
            var credencial = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.NombreDeUsuario), //no meter contraseña, ya que es facil de ver lo que trae el token
                new Claim(ClaimTypes.GivenName, user.Nombre),
                new Claim(ClaimTypes.Surname, user.Apellido),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, user.RolId.ToString()),
            };
            //crear token
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credencial
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<APIResponse> LoginUsuario(Login userAndPass)
        {
            try
            {
                var user = await _repositoryUsuario.GetByName(userAndPass.Usuario);
                if (user == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El usuario que ingreso no existe.");
                    return _apiresponse;
                }
                //var passwordVerify = user.Contraseña;
                if (!Encrypt.VerifyPassword(userAndPass.Contraseña, user.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
                _logger.LogInformation("Inicio de sesión correcto");
                var token = GenerarTokendeLogin(user);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Token = token;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar ingresar a su usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    }
}
