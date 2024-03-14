using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServicePersonaje : IServicePersonaje
    {
        private readonly IRepositoryGeneric<Personaje> _repository;
        private readonly IRepositoryGeneric<Arma> _repositoryArma;
        private readonly IRepositoryGeneric<EstiloDePelea> _repositoryEstiloDePelea;
        private readonly IRepositoryGeneric<Clan> _repositoryClan;
        private readonly IRepositoryGeneric<Reino> _repositoryReino;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServicePersonaje> _logger;
        private readonly IValidator<PersonajeCreateDto> _validator;
        private readonly IValidator<PersonajeUpdateDto> _validatorUpdate;
        public ServicePersonaje(IMapper mapper, APIResponse apiresponse, ILogger<ServicePersonaje> logger, IRepositoryGeneric<Personaje> repository, IValidator<PersonajeCreateDto> validator, 
                                IValidator<PersonajeUpdateDto> validatorUpdate, IRepositoryGeneric<Arma> repositoryArma, IRepositoryGeneric<EstiloDePelea> repositoryEstiloDePelea,
                                IRepositoryGeneric<Clan> repositoryClan, IRepositoryGeneric<Reino> repositoryReino)
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

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var personaje = await _repository.GetById(id);
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetByName(string name)
        {
            try
            {
                var personaje = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                List<Personaje> listPersonajes = await _repository.GetAll();
                Utils.ListCorrectResponse<PersonajeDto, Personaje>(_mapper, listPersonajes, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Create([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(personajeCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existPersonaje = await _repository.GetByName(personajeCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfObjectExist<Personaje>(existPersonaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var verifyReinoId = await _repositoryReino.GetById(personajeCreateDto.ReinoId);
                var verifyClanId = await _repositoryClan.GetById(personajeCreateDto.ClanId); //si es null da falso
                if (!Utils.CheckIfNull(verifyReinoId, _apiresponse, _logger))
                {
                    _logger.LogError("El id del reino al que pertenece no se encuentra registrado");
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(verifyClanId, _apiresponse, _logger))
                {
                    _logger.LogError("El id del clan al que pertenece no se encuentra registrado");
                    return _apiresponse;
                }
                var personaje = _mapper.Map<Personaje>(personajeCreateDto);
                personaje!.FechaCreacion = DateTime.Now;
                await _repository.Create(personaje);
                _logger.LogInformation("¡Personaje creado con exito!");
                Utils.CorrectResponse<PersonajeUpdateDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Delete(int id)
        {
            try
            {
                var personaje = await _repository.GetById(id); ;
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                await _repository.Delete(personaje);
                _logger.LogInformation("El personaje fue eliminado con exito.");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Update([FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(personajeUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var personaje = await _repository.GetById(personajeUpdateDto.Id);
                if (!Utils.CheckIfNull<Personaje>(personaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var verifyReinoId = await _repositoryReino.GetById(personajeUpdateDto.ReinoId);
                var verifyClanId = await _repositoryClan.GetById(personajeUpdateDto.ClanId);
                if (!Utils.CheckIfNull(verifyReinoId, _apiresponse, _logger))
                {
                    _logger.LogError("El id del reino al que pertenece no se encuentra registrado;");
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(verifyClanId, _apiresponse, _logger))
                {
                    _logger.LogError("El id del clan al que pertenece no se encuentra registrado;");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(personajeUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Personaje>(registredName, personaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(personajeUpdateDto, personaje);
                personaje.FechaActualizacion = DateTime.Now;
                await _repository.Update(personaje);
                _logger.LogInformation("¡Personaje Actualizado con exito!");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> AddWeapon(int idPersonaje, int idArma)
        {
            try
            {
                var arma = await _repositoryArma.GetById(idArma);
                var personaje = await _repository.GetById(idPersonaje);
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del arma ingresada.");                                        
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                if (Utils.VerifyIfCharacterContains(personaje, idArma, _apiresponse))
                {
                    _logger.LogError("El personaje de id " + idPersonaje + " ya cuenta con el arma de id " + idArma + ".");
                    return _apiresponse;
                }
                personaje.Armas!.Add(arma);
                await _repository.Save();
                _logger.LogInformation("Arma agregada con exito a personaje.");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> RemoveWeapon(int idPersonaje, int idArma)
        {
            try
            {
                var arma = await _repositoryArma.GetById(idArma);
                var personaje = await _repository.GetById(idPersonaje);
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del arma ingresada.");
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                if (Utils.VerifyIfCharacterNotContains(personaje, idArma, _apiresponse))
                {
                    _logger.LogError("El personaje no cuenta con ese arma.");
                    return _apiresponse;
                }
                personaje.Armas.Remove(arma);
                await _repository.Save();
                _logger.LogInformation("Arma removida con exito del personaje.");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> AddStyle(int idPersonaje, int idEstiloDePelea)
        {
            try
            {
                var estilo = await _repositoryEstiloDePelea.GetById(idEstiloDePelea);
                var personaje = await _repository.GetById(idPersonaje);
                if (!Utils.CheckIfNull(estilo, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del arma ingresada.");
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                if (Utils.VerifyIfCharacterContains(personaje, idEstiloDePelea, _apiresponse))
                {
                    _logger.LogError("El personaje de id " + idPersonaje + " ya cuenta con el estilo de pelea de id " + idEstiloDePelea + ".");
                    return _apiresponse;
                }
                personaje.EstilosDePeleas!.Add(estilo);
                await _repository.Save();
                _logger.LogInformation("Estilo de pelea agregado con exito a personaje.");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> RemoveStyle(int idPersonaje, int idEstiloDePelea)
        {
            try
            {
                var estilo = await _repositoryEstiloDePelea.GetById(idEstiloDePelea);
                var personaje = await _repository.GetById(idPersonaje);
                if (!Utils.CheckIfNull(estilo, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del arma ingresada.");
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                if (Utils.VerifyIfCharacterNotContains(personaje, idEstiloDePelea, _apiresponse))
                {
                    _logger.LogError("El personaje no cuenta con ese estilo de pelea.");
                    return _apiresponse;
                }
                personaje.EstilosDePeleas.Remove(estilo);
                await _repository.Save();
                _logger.LogInformation("Estilo de pelea removido con exito del personaje.");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto)
        {
            try
            {
                var personaje = await _repository.GetById(id);
                if (!Utils.CheckIfNull(personaje, _apiresponse, _logger))
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
                if (!Utils.CheckIfNameAlreadyExist<Personaje>(registredName, personaje, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var verifyReinoId = await _repositoryReino.GetById(personaje.ReinoId);
                var verifyClanId = await _repositoryClan.GetById(personaje.ClanId);
                if (!Utils.CheckIfNull(verifyReinoId, _apiresponse, _logger))
                {
                    _logger.LogError("El id del reino al que pertenece no se encuentra registrado;");
                    return _apiresponse;
                }
                if (!Utils.CheckIfNull(verifyClanId, _apiresponse, _logger))
                {
                    _logger.LogError("El id del clan al que pertenece no se encuentra registrado;");
                    return _apiresponse;
                }
                _mapper.Map(updatePersonajeDto, personaje);
                personaje.FechaActualizacion = DateTime.Now;
                await _repository.Update(personaje);
                _logger.LogInformation("¡Personaje Actualizado con exito!");
                Utils.CorrectResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    }
}
