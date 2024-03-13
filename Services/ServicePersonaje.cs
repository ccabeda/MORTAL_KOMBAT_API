﻿using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service.IService;
using API_MortalKombat.Services.Utils;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Service
{
    public class ServicePersonaje : IServicePersonaje
    {
        private readonly IRepositoryPersonaje _repository;
        private readonly IRepositoryArma _repositoryArma;
        private readonly IRepositoryEstiloDePelea _repositoryEstiloDePelea;
        private readonly IRepositoryClan _repositoryClan;
        private readonly IRepositoryReino _repositoryReino;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServicePersonaje> _logger;
        private readonly IValidator<PersonajeCreateDto> _validator;
        private readonly IValidator<PersonajeUpdateDto> _validatorUpdate;
        public ServicePersonaje(IMapper mapper, APIResponse apiresponse, ILogger<ServicePersonaje> logger, IRepositoryPersonaje repository, IValidator<PersonajeCreateDto> validator, 
                                IValidator<PersonajeUpdateDto> validatorUpdate, IRepositoryArma repositoryArma, IRepositoryEstiloDePelea repositoryEstiloDePelea, 
                                IRepositoryClan repositoryClan, IRepositoryReino repositoryReino)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            _repositoryArma = repositoryArma;
            _repositoryEstiloDePelea = repositoryEstiloDePelea;
            _repositoryClan = repositoryClan;
            _repositoryReino = repositoryReino;
        }

        public async Task<APIResponse> GetPersonajeById(int id)
        {
            try
            {
                var personaje = await _repository.GetById(id);
                if (Utils.CheckIfNull(personaje, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetPersonajeByName(string name)
        {
            try
            {
                var personaje = await _repository.GetByName(name);
                if (Utils.CheckIfNull(personaje, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetPersonajes()
        {
            try
            {
                List<Personaje> listCharacters = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<PersonajeDtoGetAll>>(listCharacters);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreatePersonaje([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(personajeCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existPersonaje = await _repository.GetByName(personajeCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existPersonaje != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un personaje con el mismo nombre.");
                    return _apiresponse;
                }
                var verifyReinoId = _repositoryReino.GetById(personajeCreateDto.ReinoId);
                var verifyClanId = _repositoryClan.GetById(personajeCreateDto.ClanId);
                if (verifyReinoId == null || verifyClanId == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    if (verifyReinoId == null)
                    {
                        _logger.LogError("El id del reino al que pertenece no se encuentra registrado;");
                    }
                    if (verifyClanId == null)
                    {
                        _logger.LogError("El id del clan al que pertenece no se encuentra registrado;");
                    }
                    return _apiresponse;
                }        
                var personaje = _mapper.Map<Personaje>(personajeCreateDto);
                personaje!.FechaCreacion = DateTime.Now;
                await _repository.Create(personaje);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<PersonajeUpdateDto>(personaje); //para mostrar los datos que quiero y no tener que crear otro Dto, uso update.
                _logger.LogInformation("¡Personaje creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeletePersonaje(int id)
        {
            try
            {
                var personaje = await _repository.GetById(id); ;
                if (Utils.CheckIfNull(personaje, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                await _repository.Delete(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("El personaje fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePersonaje(int id, [FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(personajeUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (id == 0 || id != personajeUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existPersonaje = await _repository.GetById(id);
                if (existPersonaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var verifyReinoId = _repositoryReino.GetById(personajeUpdateDto.ReinoId);
                var verifyClanId = _repositoryClan.GetById(personajeUpdateDto.ClanId);
                if (verifyReinoId == null || verifyClanId == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    if (verifyReinoId == null)
                    {
                        _logger.LogError("El id del reino al que pertenece no se encuentra registrado;");
                    }
                    if (verifyClanId == null)
                    {
                        _logger.LogError("El id del clan al que pertenece no se encuentra registrado;");
                    }
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(personajeUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id == personajeUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un personaje con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(personajeUpdateDto, existPersonaje);
                existPersonaje.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(existPersonaje);
                _logger.LogInformation("¡Personaje Actualizado con exito!");
                await _repository.Update(existPersonaje);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> AddWeaponToPersonaje(int idPersonaje, int idArma)
        {
            try
            {
                if (idPersonaje == 0 || idArma ==0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var arma = await _repositoryArma.GetById(idArma);
                var personaje = await _repository.GetById(idPersonaje);
                if (arma == null || personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    if (arma == null)
                    {
                        _logger.LogError("Error con el id del arma ingresada.");
                    }
                    if (personaje == null)
                    {
                        _logger.LogError("Error con el id del personaje ingresada.");
                    }
                    return _apiresponse;
                }
                if (personaje.Armas.Any(arma => arma.Id == idArma))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El personaje de id " + idPersonaje + " ya cuenta con el arma de id " + idArma + ".");
                    return _apiresponse;
                }
                personaje.Armas!.Add(arma);
                await _repository.Save();
                _logger.LogInformation("Arma agregada con exito a personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje); ;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> RemoveWeaponToPersonaje(int idPersonaje, int idArma)
        {
            try
            {
                if (idPersonaje == 0 || idArma == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var arma = await _repositoryArma.GetById(idArma);
                var personaje = await _repository.GetById(idPersonaje);
                if (arma == null || personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    if (arma == null)
                    {
                        _logger.LogError("Error con el id del arma ingresada.");
                    }
                    if (personaje == null)
                    {
                        _logger.LogError("Error con el id del personaje ingresada.");
                    }
                    return _apiresponse;
                }
                if (!personaje.Armas.Any(arma => arma.Id == idArma))
                    {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El personaje no cuenta con ese arma.");
                    return _apiresponse;
                }
                personaje.Armas.Remove(arma);
                await _repository.Save();
                _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("Arma removida con exito del personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> AddStyleToPersonaje(int idPersonaje, int idEstiloDePelea)
        {
            try
            {
                if (idPersonaje == 0 || idEstiloDePelea == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var estilo = await _repositoryEstiloDePelea.GetById(idEstiloDePelea);
                var personaje = await _repository.GetById(idPersonaje);
                if (estilo == null || personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    if (estilo == null)
                    {
                        _logger.LogError("Error con el id del estilo de pelea ingresada.");
                    }
                    if (personaje == null)
                    {
                        _logger.LogError("Error con el id del personaje ingresada.");
                    }
                    return _apiresponse;
                }
                if (personaje.EstilosDePeleas.Any(estilo => estilo.Id == idEstiloDePelea))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El personaje de id " + idPersonaje + " ya cuenta con el estilo de pelea de id " + idEstiloDePelea + ".");
                    return _apiresponse;
                }
                personaje.EstilosDePeleas!.Add(estilo);
                await _repository.Save();
                _logger.LogInformation("Estilo de pelea agregado con exito a personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje); ;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> RemoveStyleToPersonaje(int idPersonaje, int idEstiloDePelea)
        {
            try
            {
                if (idPersonaje == 0 || idEstiloDePelea == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var estilo = await _repositoryEstiloDePelea.GetById(idEstiloDePelea);
                var personaje = await _repository.GetById(idPersonaje);
                if (estilo == null || personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    if (estilo == null)
                    {
                        _logger.LogError("Error con el id del estilo de pelea ingresada.");
                    }
                    if (personaje == null)
                    {
                        _logger.LogError("Error con el id del personaje ingresada.");
                    }
                    return _apiresponse;
                }
                if (!personaje.EstilosDePeleas.Any(estilo => estilo.Id == idEstiloDePelea))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El personaje no cuenta con ese estilo de pelea.");
                    return _apiresponse;
                }
                personaje.EstilosDePeleas.Remove(estilo);
                await _repository.Save();
                _logger.LogInformation("Estilo de pelea removido con exito del personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialPersonaje(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto)
        {
            try
            {
                var personaje = await _repository.GetById(id);
                if (Utils.CheckIfNull(personaje, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var updatePersonajeDto = _mapper.Map<PersonajeUpdateDto>(personaje);
                personajeUpdateDto.ApplyTo(updatePersonajeDto!);
                if (await Utils.FluentValidator(updatePersonajeDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updatePersonajeDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != personaje.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un personaje con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(updatePersonajeDto, personaje);
                personaje.FechaActualizacion = DateTime.Now;
                await _repository.Update(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = updatePersonajeDto;
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
