using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Identity;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Service
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceUsuario> _logger;
        private readonly IValidator<UsuarioCreateDto> _validator;
        private readonly IValidator<UsuarioUpdateDto> _validatorUpdate;
        public ServiceUsuario(IMapper mapper, APIResponse apiresponse, ILogger<ServiceUsuario> logger, IRepositoryUsuario repository, IValidator<UsuarioCreateDto> validator,
            IValidator<UsuarioUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetUsuarios()
        {
            try
            {
                IEnumerable<Usuario> listUsuarios = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<UsuarioDto>>(listUsuarios);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _repository.GetById(id);
                if (Utils.CheckIfNull(usuario, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<UsuarioDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetUsuarioByName(string name)
        {
            try
            {
                var usuario = await _repository.GetByName(name);
                if (Utils.CheckIfNull(usuario, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<UsuarioDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateMyUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(usuarioCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existUsuario = await _repository.GetByName(usuarioCreateDto.NombreDeUsuario);
                if (existUsuario != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un usuario con el mismo nombre.");
                    return _apiresponse;
                }
                var usuario = _mapper.Map<Usuario>(usuarioCreateDto);
                usuario!.FechaCreacion = DateTime.Now;
                usuario.RolId = 3; //todos los usuarios se crean con el rol publico
                usuario.Contraseña = Encrypt.EncryptPassword(usuario.Contraseña); //encripto contraseña
                await _repository.Create(usuario);
                var result =_mapper.Map<UsuarioGetDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = result;
                _logger.LogInformation("¡Usuario creado con exito!");   
            }
            catch (Exception ex) 
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> ADMIN_DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _repository.GetById(id);
                if (Utils.CheckIfNull(usuario, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                await _repository.Delete(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioDto>(usuario);
                _logger.LogInformation("!Usuario eliminado con exito¡");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateMyUsuario([FromBody] UsuarioUpdateDto usuarioUpdateDto, string username, string password)
        {
            try
            {
                if (await Utils.FluentValidator(usuarioUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var usuario = await _repository.GetByName(username);
                if (Utils.CheckIfNull(usuario, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (!Encrypt.VerifyPassword(password, usuario.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
               var registredNewName = await _repository.GetByName(usuarioUpdateDto.NombreDeUsuario);
                if (registredNewName != null && registredNewName.Id != usuario.Id )
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un usuario con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(usuarioUpdateDto, usuario);
                //ENCRIPTAR CONTRASEÑA
                usuario.Contraseña = Encrypt.EncryptPassword(usuario.Contraseña);
                usuario.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioGetDto>(usuario);
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                await _repository.Update(usuario);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialMyUsuario(JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto, string username, string password)
        {
            try
            {
                var usuario = await _repository.GetByName(username);
                if (Utils.CheckIfNull(usuario, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (!Encrypt.VerifyPassword(password, usuario.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
                var updateUsuarioDto = _mapper.Map<UsuarioUpdateDto>(usuario); //mapeo el usuario a usuarioDTO
                usuarioUpdateDto.ApplyTo(updateUsuarioDto);
                if (await Utils.FluentValidator(updateUsuarioDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateUsuarioDto.NombreDeUsuario); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != usuario.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un usuario con el mismo nombre de usuario.");
                    return _apiresponse;
                }
                var encryptPassword = usuario.Contraseña;
                _mapper.Map(updateUsuarioDto, usuario);
                if (usuario.Contraseña != encryptPassword) //encripto contraseña
                {
                    usuario.Contraseña = Encrypt.EncryptPassword(updateUsuarioDto.Contraseña);
                }
                _logger.LogInformation("¡Dato cambiado con exito!");
                usuario.FechaActualizacion = DateTime.Now;
                await _repository.Update(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioGetDto>(usuario);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteMyUsuario(string username, string password)
        {
            try
            {
                var usuario = await _repository.GetByName(username);
                if (Utils.CheckIfNull(usuario, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (!Encrypt.VerifyPassword(password, usuario.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
                await _repository.Delete(usuario);
                var result = _mapper.Map<UsuarioGetDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = result;
                _logger.LogInformation("¡Usuario eliminado con exito!");
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    }
}
