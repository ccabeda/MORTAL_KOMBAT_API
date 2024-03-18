using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Models.DTOs.RolDTO;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Service
{
    public class ServiceRol : IServiceGeneric<RolUpdateDto, RolCreateDto>
    {
        private readonly IRepositoryGeneric<Rol> _repository;
        private readonly IRepositoryGeneric<Usuario> _repositoryUsuario;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceRol> _logger;
        public ServiceRol(IMapper mapper, APIResponse apiresponse, ILogger<ServiceRol> logger, IRepositoryGeneric<Rol> repository, IRepositoryGeneric<Usuario> repositoryUsuario)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                List<Rol> listRoles = await _repository.GetAll();
                if (!Utils.CheckIfLsitIsNull<Rol>(listRoles, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.ListCorrectResponse<RolDto, Rol>(_mapper, listRoles, _apiresponse);
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
                var rol = await _repository.GetById(id);
                if (!Utils.CheckIfNull(rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
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
                var rol = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] RolCreateDto rolCreateDto)
        {
            try
            {
                var existRol = await _repository.GetByName(rolCreateDto.Nombre);
                if (!Utils.CheckIfObjectExist<Rol>(existRol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var rol = _mapper.Map<Rol>(rolCreateDto);
                rol!.FechaCreacion = DateTime.Now;
                await _repository.Create(rol);
                _logger.LogInformation("¡Rol creado con exito!");
                return Utils.CorrectResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Delete(int id)
        {
            try
            {
                var rol = await _repository.GetById(id);
                if (!Utils.CheckIfNull(rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var listUsuarios = await _repositoryUsuario.GetAll();
                if (!Utils.PreventDeletionIfRelatedUserExist(listUsuarios, _apiresponse, id))
                {
                    _logger.LogError("El rol no se puede eliminar porque hay un usuario que contiene como RolId este rol.");
                    return _apiresponse;
                }
                await _repository.Delete(rol);
                _logger.LogInformation("!Rol eliminado con exito¡");
                return Utils.CorrectResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] RolUpdateDto rolUpdateDto)
        {
            try
            {
                var rol = await _repository.GetById(rolUpdateDto.Id);
                if (!Utils.CheckIfNull<Rol>(rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(rolUpdateDto.Nombre);
                if (!Utils.CheckIfNameAlreadyExist<Rol>(registredName, rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(rolUpdateDto, rol);
                rol.FechaActualizacion = DateTime.Now;
                await _repository.Update(rol);
                _logger.LogInformation("¡Rol Actualizado con exito!");
                return Utils.CorrectResponse<RolDto, Rol>(_mapper, rol, _apiresponse);                
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<RolUpdateDto> rolUpdateDto)
        {
            try
            {
                var rol = await _repository.GetById(id);
                if (!Utils.CheckIfNull(rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var updateRolDto = _mapper.Map<RolUpdateDto>(rol);
                rolUpdateDto.ApplyTo(updateRolDto!);
                var registredName = await _repository.GetByName(updateRolDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Rol>(registredName, rol, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(updateRolDto, rol);
                rol.FechaActualizacion = DateTime.Now;
                await _repository.Update(rol);
                _logger.LogInformation("¡Rol Actualizado con exito!");
                return Utils.CorrectResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
