using API_MortalKombat.Models;
using API_MortalKombat.Repository;
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
        private readonly IRepositoryLogin _repository;
        private readonly ILogger _logger;
        private readonly APIResponse _apiresponse;
        private readonly IConfiguration _config;
        public ServiceLogin(IRepositoryLogin repository, IConfiguration config, ILogger<ServiceLogin> logger, APIResponse response)
        {
            _repository = repository;
            _config = config;
            _logger = logger;
            _apiresponse = response;
        }

        public string GenerarTokendeLogin(Usuario usuario)
        {
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credencial = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.NombreDeUsuario),
                new Claim(ClaimTypes.GivenName, usuario.Nombre),
                new Claim(ClaimTypes.Surname, usuario.Apellido),
                new Claim(ClaimTypes.Email, usuario.Mail),
                new Claim(ClaimTypes.Role, usuario.RolId.ToString()),
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

        public async Task<APIResponse> LoginUsuario(Login login)
        {
            try
            {
                var usuario = await _repository.Autenticar(login);
                if (usuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Usuario o contraseña incorrecta.");
                    return _apiresponse;
                }
                var token = GenerarTokendeLogin(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Token = token;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el reino: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    }
}
