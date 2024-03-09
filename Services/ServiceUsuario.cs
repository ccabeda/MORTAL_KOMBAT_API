using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Models.DTOs.ReinoDTO;

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
                if (name == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se ingreso un nombre.");
                    return _apiresponse;
                }
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

        public async Task<APIResponse> CreateUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(usuarioCreateDto); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
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

        public async Task<APIResponse> DeleteUsuario(int id)
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

        public async Task<APIResponse> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto usuarioUpdateDto)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(usuarioUpdateDto); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (id == 0 || id != usuarioUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existUsuario = await _repository.GetById(id);
                if (existUsuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(usuarioUpdateDto.NombreDeUsuario);
                if (registredName != null && registredName.Id != usuarioUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un usuario con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(usuarioUpdateDto, existUsuario);
                existUsuario.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioDto>(existUsuario);
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                await _repository.Update(existUsuario);
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

        public async Task<APIResponse> UpdatePartialUsuario(int id, JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto)
        {
            try
            {
                var usuario = await _repository.GetById(id);
                if (usuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                    return _apiresponse;
                }
                var usuarioDTO = _mapper.Map<UsuarioUpdateDto>(usuario);
                usuarioUpdateDto.ApplyTo(usuarioDTO!);
                var fluentValidation = await _validatorUpdate.ValidateAsync(usuarioDTO!);
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;

                }
                _mapper.Map(usuarioDTO, usuario);
                usuario.FechaActualizacion = DateTime.Now;
                await _repository.Update(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = usuarioDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el usuario de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }
    }
}
