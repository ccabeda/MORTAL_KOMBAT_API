using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using System.Net;

namespace API_MortalKombat.Service
{
    public class ServiceLogin : IServiceLogin
    {
        private readonly IRepositoryGeneric<Usuario> _repositoryUsuario;
        private readonly ILogger _logger;
        private readonly APIResponse _apiresponse;
        private readonly IConfiguration _config;
        public ServiceLogin(IRepositoryGeneric<Usuario> repositoryUsuario, IConfiguration config, ILogger<ServiceLogin> logger, APIResponse response)
        {
            _repositoryUsuario = repositoryUsuario;
            _config = config;
            _logger = logger;
            _apiresponse = response;
        }

        public async Task<APIResponse> LoginUsuario(Login userAndPass)
        {
            try
            {
                var usuario = await _repositoryUsuario.GetByName(userAndPass.Usuario);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                if (!Utils.VerifyPassword(userAndPass.Contraseña, usuario.Contraseña, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _logger.LogInformation("Inicio de sesión correcto");
                var token = Utils.GenerarTokendeLogin(usuario, _config);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Token = "Bearer " + token;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
