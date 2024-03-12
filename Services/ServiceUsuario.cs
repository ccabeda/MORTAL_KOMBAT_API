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
                _logger.LogError("Ocurrió un error al obtener la lista de Usuarios: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _repository.GetById(id);
                if (usuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id " + id + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<UsuarioDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetUsuarioByName(string name)
        {
            try
            {
                var usuario = await _repository.GetByName(name);
                if (usuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El usuario " + name + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<UsuarioDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateMyUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            try
            {
                if (Utils.FluentValidator(usuarioCreateDto, _validator, _apiresponse, _logger) != null)
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
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> ADMIN_DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _repository.GetById(id);
                if (usuario == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El usuario no se encuentra registrado. Verifica que el id ingresado sea correcto.");
                    _apiresponse.isExit = false;
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
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateMyUsuario([FromBody] UsuarioUpdateDto usuarioUpdateDto, string username, string password)
        {
            try
            {
                if (Utils.FluentValidator(usuarioUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var user = await _repository.GetByName(username);
                if (user == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El usuario que ingreso no existe.");
                    return _apiresponse;
                }
                if (!Encrypt.VerifyPassword(password, user.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
               var registredNewName = await _repository.GetByName(usuarioUpdateDto.NombreDeUsuario);
                if (registredNewName != null && registredNewName.Id != user.Id )
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un usuario con el mismo nombre.");
                    return _apiresponse;
                }
                //ENCRIPTAR CONTRASEÑA EN CASO DE QUE LA CAMBIE
                if (user.Contraseña != usuarioUpdateDto.Contraseña)
                {
                    usuarioUpdateDto.Contraseña = Encrypt.EncryptPassword(usuarioUpdateDto.Contraseña); //encripto contraseña
                }
                _mapper.Map(usuarioUpdateDto, user);
                user.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioGetDto>(user);
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                await _repository.Update(user);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar al Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialMyUsuario(JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto, string username, string password)
        {
            try
            {
                var user = await _repository.GetByName(username);
                if (user == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El usuario que ingreso no existe.");
                    return _apiresponse;
                }
                if (!Encrypt.VerifyPassword(password, user.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
                var updateUsuarioDto = _mapper.Map<UsuarioUpdateDto>(user); //mapeo el usuario a usuarioDTO
                usuarioUpdateDto.ApplyTo(updateUsuarioDto);
                if (Utils.FluentValidator(updateUsuarioDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (updateUsuarioDto.Contraseña != user.Contraseña) //si cambio la contraseña, la encripto 
                {
                    updateUsuarioDto.Contraseña = Encrypt.EncryptPassword(updateUsuarioDto.Contraseña);
                }
                _mapper.Map(updateUsuarioDto, user);
                _logger.LogInformation("¡Dato cambiado con exito!");
                user.FechaActualizacion = DateTime.Now;
                await _repository.Update(user);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioGetDto>(user);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el usuario. Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteMyUsuario(string username, string password)
        {
            try
            {
                var user = await _repository.GetByName(username);
                if (user == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Usuario incorrecto.");
                    return _apiresponse;
                }
                if (!Encrypt.VerifyPassword(password, user.Contraseña))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Contraseña incorrecta");
                    return _apiresponse;
                }
                await _repository.Delete(user);
                var result = _mapper.Map<UsuarioGetDto>(user);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = result;
                _logger.LogInformation("¡Usuario eliminado con exito!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar eliminar el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    }
}
