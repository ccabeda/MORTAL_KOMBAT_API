﻿using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Models.DTOs.RolDTO;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using FluentValidation;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Service
{
    public class ServiceRol : IServiceRol
    {
        private readonly IRepositoryRol _repository;
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceRol> _logger;
        private readonly IValidator<RolCreateDto> _validator;
        private readonly IValidator<RolUpdateDto> _validatorUpdate;
        public ServiceRol(IMapper mapper, APIResponse apiresponse, ILogger<ServiceRol> logger, IRepositoryRol repository, IRepositoryUsuario repositoryUsuario, IValidator<RolCreateDto> 
                          validator, IValidator<RolUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetRoles()
        {
            try
            {
                List<Rol> listRoles = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<RolDto>>(listRoles);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetRolById(int id)
        {
            try
            {
                var rol = await _repository.GetById(id);
                if (Utils.CheckIfNull(rol, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetRolByName(string name)
        {
            try
            {
                var rol = await _repository.GetByName(name);
                if (Utils.CheckIfNull(rol, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateRol([FromBody] RolCreateDto rolCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(rolCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existeRol = await _repository.GetByName(rolCreateDto.Nombre);
                if (existeRol != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un rol con el mismo nombre.");
                    return _apiresponse;
                }
                var rol = _mapper.Map<Rol>(rolCreateDto);
                rol!.FechaCreacion = DateTime.Now;
                await _repository.Create(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _logger.LogInformation("¡Rol creado con exito!");
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteRol(int id)
        {
            try
            {
                var rol = await _repository.GetById(id);
                if (Utils.CheckIfNull(rol, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var listUsuarios = await _repositoryUsuario.GetAll();
                foreach (var i in listUsuarios) //aqui podria usarse el metodo cascada para que se borre todo, pero decidi agergarle esto para mas seguridad
                {
                    if (i.RolId == id)
                    {
                        _apiresponse.statusCode = HttpStatusCode.NotFound;
                        _logger.LogError("El Rol no se puede eliminar porque el usuario " + i.NombreDeUsuario + " de id " + i.Id + " contiene como RolId este rol.");
                        _apiresponse.isExit = false;
                        return _apiresponse;
                    }
                }
                await _repository.Delete(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<RolDto>(rol);
                _logger.LogInformation("!Rol eliminado con exito¡");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateRol(int id, [FromBody] RolUpdateDto rolUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(rolUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (id == 0 || id != rolUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existRol = await _repository.GetById(id);
                if (existRol == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(rolUpdateDto.Nombre);
                if (registredName != null && registredName.Id != rolUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict;
                    _logger.LogError("Ya existe un rol con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(rolUpdateDto, existRol);
                existRol.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<RolDto>(existRol);
                _logger.LogInformation("¡Rol Actualizado con exito!");
                await _repository.Update(existRol);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialRol(int id, JsonPatchDocument<RolUpdateDto> rolUpdateDto)
        {
            try
            {
                var rol = await _repository.GetById(id);
                if (Utils.CheckIfNull(rol, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var updateRolDto = _mapper.Map<RolUpdateDto>(rol);
                rolUpdateDto.ApplyTo(updateRolDto!);
                if (await Utils.FluentValidator(updateRolDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateRolDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != rol.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un rol con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(updateRolDto, rol);
                rol.FechaActualizacion = DateTime.Now;
                await _repository.Update(rol);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = updateRolDto;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    }
}
