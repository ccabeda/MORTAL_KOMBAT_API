using API_MortalKombat.Models;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;
using System.Net;

namespace API_MortalKombat.Service
{
    public class ServiceLogin : IServiceLogin
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly APIResponse _apiresponse;
        private readonly IConfiguration _config;
        public ServiceLogin(IUnitOfWork unitOfWork, IConfiguration config, ILogger<ServiceLogin> logger, APIResponse response)
        {
            _config = config;
            _logger = logger;
            _apiresponse = response;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> LoginUsuario(Login userAndPass)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetByName(userAndPass.Usuario);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("Usuario incorrecto.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.VerifyPassword(userAndPass.Contraseña, usuario.Contraseña))
                {
                    _logger.LogError("Contraseña Incorrecta.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                _logger.LogInformation("Inicio de sesión correcto.");
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
