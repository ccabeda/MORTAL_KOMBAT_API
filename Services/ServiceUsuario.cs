using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using API_MortalKombat.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;

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
                IEnumerable<Usuario> lista_usuarios = await _repository.ObtenerTodos();
                _apiresponse.Result = _mapper.Map<IEnumerable<UsuarioDto>>(lista_usuarios);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Usuarios: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetUsuarioById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiresponse.isExit=false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var usuario = await _repository.ObtenerPorId(id);
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
                var usuario = await _repository.ObtenerPorNombre(name);
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
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            try
            {
                var fluent_validation = await _validator.ValidateAsync(usuarioCreateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (usuarioCreateDto == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var existeUsuario = await _repository.ObtenerPorNombre(usuarioCreateDto.NombreDeUsuario);
                if (existeUsuario != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un usuario con el mismo nombre.");
                    return _apiresponse;
                }
                var usuario = _mapper.Map<Usuario>(usuarioCreateDto);
                usuario.FechaCreacion = DateTime.Now;
                usuario.RolId = 3; //todos los usuarios se crean con el rol publico
                await _repository.Crear(usuario);
                var result =_mapper.Map<UsuarioGetDto>(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = result;
                _logger.LogInformation("¡Usuario creado con exito!");   
            }
            catch (Exception ex) 
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteUsuario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var usuario = await _repository.ObtenerPorId(id);
                if (usuario == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El usuario no se encuentra registrado.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Eliminar(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioDto>(usuario);
                _logger.LogInformation("!Usuario eliminado con exito¡");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto usuarioUpdateDto)
        {
            try
            {
                var fluent_validation = await _validatorUpdate.ValidateAsync(usuarioUpdateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
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
                var existeUsuario = await _repository.ObtenerPorId(id);
                if (existeUsuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var nombre_ya_registrado = await _repository.ObtenerPorNombre(usuarioUpdateDto.NombreDeUsuario);
                if (nombre_ya_registrado != null && nombre_ya_registrado.Id != usuarioUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un usuario con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(usuarioUpdateDto, existeUsuario);
                existeUsuario.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<UsuarioDto>(existeUsuario);
                _logger.LogInformation("¡Usuario Actualizado con exito!");
                await _repository.Actualizar(existeUsuario);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar al Usuario: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialUsuario(int id, JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto)
        {
            try
            {
                if (usuarioUpdateDto == null || id == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Error al ingresar los datos.");
                    return _apiresponse;
                }
                var usuario = await _repository.ObtenerPorId(id);
                if (usuario == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                    return _apiresponse;
                }
                var usuarioDTO = _mapper.Map<UsuarioUpdateDto>(usuario);
                usuarioUpdateDto.ApplyTo(usuarioDTO);
                _mapper.Map(usuarioDTO, usuario);
                usuario.FechaActualizacion = DateTime.Now;
                await _repository.Actualizar(usuario);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = usuarioDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el usuario de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }
    }
}
