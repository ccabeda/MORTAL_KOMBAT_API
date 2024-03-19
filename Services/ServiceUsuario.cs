using API_MortalKombat.Models;
using AutoMapper;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;

namespace API_MortalKombat.Service
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceUsuario> _logger;
        public ServiceUsuario(IMapper mapper, APIResponse apiresponse, ILogger<ServiceUsuario> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                List<Usuario> listUsuarios = await _unitOfWork.repositoryUsuario.GetAll();
                if (Utils.CheckIfLsitIsNull<Usuario>(listUsuarios))
                {
                    _logger.LogError("La lista de usuarios esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<UsuarioDto, Usuario>(_mapper, listUsuarios, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetById(id);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("El usuario de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> GetByName(string name)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetByName(name);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("El usuario de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> CreateMyUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            try
            {
                var existUsuario = await _unitOfWork.repositoryUsuario.GetByName(usuarioCreateDto.NombreDeUsuario);
                if (Utils.CheckIfObjectExist<Usuario>(existUsuario))
                {
                    _logger.LogError("El nombre del usuario ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var usuario = _mapper.Map<Usuario>(usuarioCreateDto);
                usuario!.FechaCreacion = DateTime.Now;
                usuario.RolId = 3; //todos los usuarios se crean con el rol publico
                usuario.Contraseña = Encrypt.EncryptPassword(usuario.Contraseña); //encripto contraseña
                await _unitOfWork.repositoryUsuario.Create(usuario);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Usuario creado con exito!");
                return Utils.OKResponse<UsuarioGetDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex) 
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> ADMIN_DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetById(id);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("El usuario de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                await _unitOfWork.repositoryUsuario.Delete(usuario);
                await _unitOfWork.Save();
                _logger.LogInformation("!Usuario eliminado con exito¡");
                return Utils.OKResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdateMyUsuario([FromBody] UsuarioUpdateDto usuarioUpdateDto, string username, string password)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetByName(username);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("Uusario incorrecto.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (!Utils.VerifyPassword(password, usuario.Contraseña))
                {
                    _logger.LogError("Contraseña Incorrecta.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryUsuario.GetByName(usuarioUpdateDto.NombreDeUsuario);
                if (Utils.CheckIfNameAlreadyExist<Usuario>(registredName, usuario))
                {
                    _logger.LogError("El nombre del usuario ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(usuarioUpdateDto, usuario);
                //ENCRIPTAR CONTRASEÑA
                usuario.Contraseña = Encrypt.EncryptPassword(usuario.Contraseña);
                usuario.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryUsuario.Update(usuario);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                return Utils.OKResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartialMyUsuario(JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto, string username, string password)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetByName(username);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("Usuario incorrecto.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (!Utils.VerifyPassword(password, usuario.Contraseña))
                {
                    _logger.LogError("Contraseña Incorrecta.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                var updateUsuarioDto = _mapper.Map<UsuarioUpdateDto>(usuario); //mapeo el usuario a usuarioDTO
                usuarioUpdateDto.ApplyTo(updateUsuarioDto);
                var registredName = await _unitOfWork.repositoryUsuario.GetByName(updateUsuarioDto.NombreDeUsuario);
                if (Utils.CheckIfNameAlreadyExist<Usuario>(registredName, usuario))
                {
                    _logger.LogError("El nombre del usuario ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var encryptPassword = usuario.Contraseña;
                _mapper.Map(updateUsuarioDto, usuario);
                if (usuario.Contraseña != encryptPassword) //encripto contraseña
                {
                    usuario.Contraseña = Encrypt.EncryptPassword(updateUsuarioDto.Contraseña);
                }
                _logger.LogInformation("¡Dato cambiado con exito!");
                usuario.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryUsuario.Update(usuario);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                return Utils.OKResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> DeleteMyUsuario(string username, string password)
        {
            try
            {
                var usuario = await _unitOfWork.repositoryUsuario.GetByName(username);
                if (Utils.CheckIfNull(usuario))
                {
                    _logger.LogError("Usuario incorrecto.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (!Utils.VerifyPassword(password, usuario.Contraseña))
                {
                    _logger.LogError("Contraseña Incorrecta.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                await _unitOfWork.repositoryUsuario.Delete(usuario);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Usuario eliminado con exito!");
                return Utils.OKResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
