using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Models.DTOs.RolDTO;
using Microsoft.AspNetCore.JsonPatch;

namespace API_MortalKombat.Service
{
    public class ServiceRol : IServiceRol
    {
        private readonly IRepositoryRol _repository;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceRol> _logger;
        public ServiceRol(IMapper mapper, APIResponse apiresponse, ILogger<ServiceRol> logger, IRepositoryRol repository, IRepositoryUsuario repositoryUsuario)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<APIResponse> GetRoles()
        {
            try
            {
                IEnumerable<Rol> lista_roles = await _repository.ObtenerTodos();
                _apiresponse.Result = _mapper.Map<IEnumerable<RolDto>>(lista_roles);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Roles: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetRolById(int id)
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
                var rol = await _repository.ObtenerPorId(id);
                if (rol == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id " + id + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Rol: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetRolByName(string name)
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
                var rol = await _repository.ObtenerPorNombre(name);
                if (rol == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El rol " + name + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Rol: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateRol([FromBody] RolCreateDto rolCreateDto)
        {
            try
            {
                if (rolCreateDto == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var existeRol = await _repository.ObtenerPorNombre(rolCreateDto.Nombre);
                if (existeRol != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un rol con el mismo nombre.");
                    return _apiresponse;
                }
                var rol = _mapper.Map<Rol>(rolCreateDto);
                rol!.FechaCreacion = DateTime.Now;
                await _repository.Crear(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _logger.LogInformation("¡Rol creado con exito!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Rol: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteRol(int id)
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
                var rol = await _repository.ObtenerPorId(id);
                if (rol == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El rol no se encuentra registrado.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                var lista_usuarios = await _repositoryUsuario.ObtenerTodos();
                foreach (var i in lista_usuarios) //aqui podria usarse el metodo cascada para que se borre todo, pero decidi agergarle esto para mas seguridad
                {
                    if (i.RolId == id)
                    {
                        _apiresponse.statusCode = HttpStatusCode.NotFound;
                        _logger.LogError("El Rol no se puede eliminar porque el usuario " + i.NombreDeUsuario + " de id " + i.Id + " contiene como RolId este rol.");
                        _apiresponse.isExit = false;
                        return _apiresponse;
                    }
                }
                await _repository.Eliminar(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _logger.LogInformation("!Rol eliminado con exito¡");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Rol: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateRol(int id, [FromBody] RolUpdateDto rolUpdateDto)
        {
            try
            {
                if (id == 0 || id != rolUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existeRol = await _repository.ObtenerPorId(id);
                if (existeRol == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var nombre_ya_registrado = await _repository.ObtenerPorNombre(rolUpdateDto.Nombre);
                if (nombre_ya_registrado != null && nombre_ya_registrado.Id != rolUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un rol con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(rolUpdateDto, existeRol);
                existeRol.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<RolDto>(existeRol);
                _logger.LogInformation("¡Rol Actualizado con exito!");
                await _repository.Actualizar(existeRol);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar al personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialRol(int id, JsonPatchDocument<RolUpdateDto> rolUpdateDto)
        {
            try
            {
                if (rolUpdateDto == null || id == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Error al ingresar los datos.");
                    return _apiresponse;
                }
                var rol = await _repository.ObtenerPorId(id);
                if (rol == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                    return _apiresponse;
                }
                var rolDTO = _mapper.Map<RolUpdateDto>(rol);
                rolUpdateDto.ApplyTo(rolDTO!);
                _mapper.Map(rolDTO, rol);
                rol.FechaActualizacion = DateTime.Now;
                await _repository.Actualizar(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = rolDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el rol de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }
    }
}
