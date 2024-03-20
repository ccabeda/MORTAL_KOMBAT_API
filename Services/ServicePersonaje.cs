using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServicePersonaje : IServicePersonaje
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServicePersonaje> _logger;
        public ServicePersonaje(IMapper mapper, APIResponse apiresponse, ILogger<ServicePersonaje> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(id);
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("El personaje de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
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
                var personaje = await _unitOfWork.repositoryPersonaje.GetByName(name);
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("El arma de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                List<Personaje> listPersonajes = await _unitOfWork.repositoryPersonaje.GetAll();
                if (Utils.CheckIfLsitIsNull<Personaje>(listPersonajes))
                {
                    _logger.LogError("La lista de personajes esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<PersonajeDto, Personaje>(_mapper, listPersonajes, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            try
            {
                var existPersonaje = await _unitOfWork.repositoryPersonaje.GetByName(personajeCreateDto.Nombre);
                if (!Utils.CheckIfNull<Personaje>(existPersonaje))
                {
                    _logger.LogError("El nombre del personaje ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var verifyReinoId = await _unitOfWork.repositoryReino.GetById(personajeCreateDto.ReinoId);
                var verifyClanId = await _unitOfWork.repositoryClan.GetById(personajeCreateDto.ClanId); //si es null da falso
                if (Utils.CheckIfNull(verifyReinoId))
                {
                    _logger.LogError("El id del reino al que pertenece no se encuentra registrado");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(verifyClanId))
                {
                    _logger.LogError("El id del clan al que pertenece no se encuentra registrado");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var personaje = _mapper.Map<Personaje>(personajeCreateDto);
                personaje!.FechaCreacion = DateTime.Now;
                await _unitOfWork.repositoryPersonaje.Create(personaje);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Personaje creado con exito!");
                return Utils.OKResponse<PersonajeUpdateDto, Personaje>(_mapper, personaje, _apiresponse);
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
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(id);
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("El personaje de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                await _unitOfWork.repositoryPersonaje.Delete(personaje);
                await _unitOfWork.Save();
                _logger.LogInformation("El personaje fue eliminado con exito.");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            try
            {
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(personajeUpdateDto.Id);
                if (Utils.CheckIfNull<Personaje>(personaje))
                {
                    _logger.LogError("El personaje de id " + personajeUpdateDto.Id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var verifyReinoId = await _unitOfWork.repositoryReino.GetById(personajeUpdateDto.ReinoId);
                var verifyClanId = await _unitOfWork.repositoryClan.GetById(personajeUpdateDto.ClanId);
                if (Utils.CheckIfNull(verifyReinoId))
                {
                    _logger.LogError("El id del clan " + personaje.ReinoId + " encuentra registrado;");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(verifyClanId))
                {
                    _logger.LogError("El id del clan " + personaje.ClanId + " encuentra registrado;");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryPersonaje.GetByName(personajeUpdateDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Personaje>(registredName, personaje))
                {
                    _logger.LogError("El nombre del personaje ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(personajeUpdateDto, personaje);
                personaje.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryPersonaje.Update(personaje);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Personaje Actualizado con exito!");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }

        }

        public async Task<APIResponse> AddWeapon(int idPersonaje, int idArma)
        {
            try
            {
                var arma = await _unitOfWork.repositoryArma.GetById(idArma);
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(idPersonaje);
                if (Utils.CheckIfNull(arma))
                {
                    _logger.LogError("Error con el id del arma ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.VerifyIfCharacterContains(arma, personaje, idArma))
                {
                    _logger.LogError("El personaje de id " + idPersonaje + " ya cuenta con el arma de id " + idArma + ".");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                personaje.Armas.Add(arma);
                await _unitOfWork.Save();
                _logger.LogInformation("Arma agregada con exito a personaje.");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> RemoveWeapon(int idPersonaje, int idArma)
        {
            try
            {
                var arma = await _unitOfWork.repositoryArma.GetById(idArma);
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(idPersonaje);
                if (Utils.CheckIfNull(arma))
                {
                    _logger.LogError("Error con el id del arma ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (!Utils.VerifyIfCharacterContains(arma, personaje, idArma))
                {
                    _logger.LogError("El personaje no cuenta con ese arma.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                personaje.Armas.Remove(arma);
                await _unitOfWork.Save();
                _logger.LogInformation("Arma removida con exito del personaje.");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> AddStyle(int idPersonaje, int idEstiloDePelea)
        {
            try
            {
                var estilo = await _unitOfWork.repositoryEstiloDePelea.GetById(idEstiloDePelea);
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(idPersonaje);
                if (Utils.CheckIfNull(estilo))
                {
                    _logger.LogError("Error con el id del estilo de pelea ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.VerifyIfCharacterContains(estilo, personaje, idEstiloDePelea))
                {
                    _logger.LogError("El personaje de id " + idPersonaje + " ya cuenta con el estilo de pelea de id " + idEstiloDePelea + ".");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                personaje.EstilosDePeleas.Add(estilo);
                await _unitOfWork.Save();
                _logger.LogInformation("Estilo de pelea agregado con exito a personaje.");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> RemoveStyle(int idPersonaje, int idEstiloDePelea)
        {
            try
            {
                var estilo = await _unitOfWork.repositoryEstiloDePelea.GetById(idEstiloDePelea);
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(idPersonaje);
                if (Utils.CheckIfNull(estilo))
                {
                    _logger.LogError("Error con el id del estilo de pelea ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (!Utils.VerifyIfCharacterContains(estilo, personaje, idEstiloDePelea))
                {
                    _logger.LogError("El personaje no cuenta con ese estilo de pelea.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                personaje.EstilosDePeleas.Remove(estilo);
                await _unitOfWork.Save();
                _logger.LogInformation("Estilo de pelea removido con exito del personaje.");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto)
        {
            try
            {
                var personaje = await _unitOfWork.repositoryPersonaje.GetById(id);
                if (Utils.CheckIfNull(personaje))
                {
                    _logger.LogError("El personaje de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var updatePersonajeDto = _mapper.Map<PersonajeUpdateDto>(personaje);
                personajeUpdateDto.ApplyTo(updatePersonajeDto!);
                var registredName = await _unitOfWork.repositoryPersonaje.GetByName(updatePersonajeDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Personaje>(registredName, personaje))
                {
                    _logger.LogError("El nombre del personaje ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var verifyReinoId = await _unitOfWork.repositoryReino.GetById(personaje.ReinoId);
                var verifyClanId = await _unitOfWork.repositoryClan.GetById(personaje.ClanId);
                if (Utils.CheckIfNull(verifyReinoId))
                {
                    _logger.LogError("El id del reino " + personaje.ReinoId + " encuentra registrado;");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                if (Utils.CheckIfNull(verifyClanId))
                {
                    _logger.LogError("El id del clan " + personaje.ClanId + " encuentra registrado;");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                _mapper.Map(updatePersonajeDto, personaje);
                personaje.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryPersonaje.Update(personaje);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Personaje Actualizado con exito!");
                return Utils.OKResponse<PersonajeDto, Personaje>(_mapper, personaje, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
