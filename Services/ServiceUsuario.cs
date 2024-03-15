using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Service
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryGeneric<Usuario> _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceUsuario> _logger;
        private readonly IValidator<UsuarioCreateDto> _validator;
        private readonly IValidator<UsuarioUpdateDto> _validatorUpdate;
        public ServiceUsuario(IMapper mapper, APIResponse apiresponse, ILogger<ServiceUsuario> logger, IRepositoryGeneric<Usuario> repository, IValidator<UsuarioCreateDto> validator,
            IValidator<UsuarioUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                List<Usuario> listUsuarios = await _repository.GetAll();
                if (!Utils.CheckIfLsitIsNull<Usuario>(listUsuarios, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.ListCorrectResponse<UsuarioDto, Usuario>(_mapper, listUsuarios, _apiresponse);
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
                var usuario = await _repository.GetById(id);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
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
                var usuario = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
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
                if (await Utils.FluentValidator(usuarioCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existUsuario = await _repository.GetByName(usuarioCreateDto.NombreDeUsuario);
                if (!Utils.CheckIfObjectExist<Usuario>(existUsuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var usuario = _mapper.Map<Usuario>(usuarioCreateDto);
                usuario!.FechaCreacion = DateTime.Now;
                usuario.RolId = 3; //todos los usuarios se crean con el rol publico
                usuario.Contraseña = Encrypt.EncryptPassword(usuario.Contraseña); //encripto contraseña
                await _repository.Create(usuario);
                _logger.LogInformation("¡Usuario creado con exito!");
                return Utils.CorrectResponse<UsuarioGetDto, Usuario>(_mapper, usuario, _apiresponse);
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
                var usuario = await _repository.GetById(id);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                await _repository.Delete(usuario);
                _logger.LogInformation("!Usuario eliminado con exito¡");
                return Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
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
                if (await Utils.FluentValidator(usuarioUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var usuario = await _repository.GetByName(username);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                if (!Utils.VerifyPassword(password, usuario.Contraseña, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(usuarioUpdateDto.NombreDeUsuario);
                if (!Utils.CheckIfNameAlreadyExist<Usuario>(registredName, usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(usuarioUpdateDto, usuario);
                //ENCRIPTAR CONTRASEÑA
                usuario.Contraseña = Encrypt.EncryptPassword(usuario.Contraseña);
                usuario.FechaActualizacion = DateTime.Now;
                await _repository.Update(usuario);
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                return Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
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
                var usuario = await _repository.GetByName(username);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                if (!Utils.VerifyPassword(password, usuario.Contraseña, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var updateUsuarioDto = _mapper.Map<UsuarioUpdateDto>(usuario); //mapeo el usuario a usuarioDTO
                usuarioUpdateDto.ApplyTo(updateUsuarioDto);
                if (await Utils.FluentValidator(updateUsuarioDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateUsuarioDto.NombreDeUsuario); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Usuario>(registredName, usuario, _apiresponse, _logger))
                {
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
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                return Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
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
                var usuario = await _repository.GetByName(username);
                if (!Utils.CheckIfNull(usuario, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                if (!Utils.VerifyPassword(password, usuario.Contraseña, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                await _repository.Delete(usuario);
                _logger.LogInformation("¡Usuario eliminado con exito!");
                return Utils.CorrectResponse<UsuarioDto, Usuario>(_mapper, usuario, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
