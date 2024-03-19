using API_MortalKombat.Models;
using API_MortalKombat.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Models.DTOs.RolDTO;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;

namespace API_MortalKombat.Service
{
    public class ServiceRol : IServiceGeneric<RolUpdateDto, RolCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceRol> _logger;
        public ServiceRol(IMapper mapper, APIResponse apiresponse, ILogger<ServiceRol> logger, IUnitOfWork unitOfWork)
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
                List<Rol> listRoles = await _unitOfWork.repositoryRol.GetAll();
                if (Utils.CheckIfLsitIsNull<Rol>(listRoles))
                {
                    _logger.LogError("La lista de roles esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<RolDto, Rol>(_mapper, listRoles, _apiresponse);
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
                var rol = await _unitOfWork.repositoryRol.GetById(id);
                if (Utils.CheckIfNull(rol))
                {
                    _logger.LogError("El rol de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
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
                var rol = await _unitOfWork.repositoryRol.GetByName(name);
                if (Utils.CheckIfNull(rol))
                {
                    _logger.LogError("El rol de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
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
                var existRol = await _unitOfWork.repositoryRol.GetByName(rolCreateDto.Nombre);
                if (Utils.CheckIfObjectExist<Rol>(existRol))
                {
                    _logger.LogError("El nombre del rol ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var rol = _mapper.Map<Rol>(rolCreateDto);
                rol!.FechaCreacion = DateTime.Now;
                await _unitOfWork.repositoryRol.Create(rol);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Rol creado con exito!");
                return Utils.OKResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
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
                var rol = await _unitOfWork.repositoryRol.GetById(id);
                if (Utils.CheckIfNull(rol))
                {
                    _logger.LogError("El rol de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var listUsuarios = await _unitOfWork.repositoryUsuario.GetAll();
                if (!Utils.PreventDeletionIfRelatedUserExist(listUsuarios, id))
                {
                    _logger.LogError("El rol no se puede eliminar porque hay un usuario que contiene como RolId este rol.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                await _unitOfWork.repositoryRol.Delete(rol);
                await _unitOfWork.Save();
                _logger.LogInformation("!Rol eliminado con exito¡");
                return Utils.OKResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
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
                var rol = await _unitOfWork.repositoryRol.GetById(rolUpdateDto.Id);
                if (Utils.CheckIfNull<Rol>(rol))
                {
                    _logger.LogError("El rol de id " + rolUpdateDto.Id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryRol.GetByName(rolUpdateDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Rol>(registredName, rol))
                {
                    _logger.LogError("El nombre del rol ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(rolUpdateDto, rol);
                rol.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryRol.Update(rol);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Rol Actualizado con exito!");
                return Utils.OKResponse<RolDto, Rol>(_mapper, rol, _apiresponse);                
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
                var rol = await _unitOfWork.repositoryRol.GetById(id);
                if (Utils.CheckIfNull(rol))
                {
                    _logger.LogError("El rol de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var updateRolDto = _mapper.Map<RolUpdateDto>(rol);
                rolUpdateDto.ApplyTo(updateRolDto!);
                var registredName = await _unitOfWork.repositoryRol.GetByName(updateRolDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Rol>(registredName, rol))
                {
                    _logger.LogError("El nombre del rol ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(updateRolDto, rol);
                rol.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryRol.Update(rol);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Rol Actualizado con exito!");
                return Utils.OKResponse<RolDto, Rol>(_mapper, rol, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
