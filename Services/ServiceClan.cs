using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServiceClan : IServiceGeneric<ClanUpdateDto, ClanCreateDto>
    {
        private readonly IRepositoryGeneric<Clan> _repository;
        private readonly IRepositoryGeneric<Personaje> _repositoryPersonaje;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceClan> _logger;
        private readonly IValidator<ClanCreateDto> _validator;
        private readonly IValidator<ClanUpdateDto> _validatorUpdate;
        public ServiceClan(IMapper mapper, APIResponse apiresponse, ILogger<ServiceClan> logger, IRepositoryGeneric<Clan> repository, IValidator<ClanCreateDto> validator, 
            IValidator<ClanUpdateDto> validatorUpdate, IRepositoryGeneric<Personaje> repositoryPersonaje)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;   
            _repositoryPersonaje = repositoryPersonaje;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var clan = await _repository.GetById(id);
                if (!Utils.CheckIfNull(clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                var clan = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                List<Clan> listClanes = await _repository.GetAll();
                if (!Utils.CheckIfLsitIsNull<Clan>(listClanes, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.ListCorrectResponse<ClanDto, Clan>(_mapper, listClanes, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] ClanCreateDto clanCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(clanCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existClan = await _repository.GetByName(clanCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfObjectExist<Clan>(existClan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var clan = _mapper.Map<Clan>(clanCreateDto);
                clan!.FechaCreacion = DateTime.Now;
                await _repository.Create(clan);
                _logger.LogInformation("¡Clan creado con exito!");
                return Utils.CorrectResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                var clan = await _repository.GetById(id); ;
                if (!Utils.CheckIfNull(clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var listPersonajes = await _repositoryPersonaje.GetAll();
                if (!Utils.PreventDeletionIfRelatedCharacterExist(clan ,listPersonajes, _apiresponse, id))
                {
                    _logger.LogError("El clan no se puede eliminar porque hay un personaje que contiene como ClanId este clan.");
                    return _apiresponse;
                }
                await _repository.Delete(clan);
                _logger.LogInformation("El clan fue eliminado con exito.");
                return Utils.CorrectResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] ClanUpdateDto clanUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(clanUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var clan = await _repository.GetById(clanUpdateDto.Id);
                if (!Utils.CheckIfNull<Clan>(clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(clanUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Clan>(registredName, clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(clanUpdateDto, clan);
                clan.FechaActualizacion = DateTime.Now;
                await _repository.Update(clan);
                _logger.LogInformation("¡Clan Actualizado con exito!");
                return Utils.CorrectResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<ClanUpdateDto> clanUpdateDto)
        {
            try
            {
                var clan = await _repository.GetById(id);
                if (!Utils.CheckIfNull(clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var updateClanDto = _mapper.Map<ClanUpdateDto>(clan); 
                clanUpdateDto.ApplyTo(updateClanDto!);
                if (await Utils.FluentValidator(updateClanDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateClanDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Clan>(registredName, clan, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(updateClanDto, clan); 
                clan.FechaActualizacion = DateTime.Now;
                await _repository.Update(clan);
                _logger.LogInformation("¡Clan Actualizado con exito!");
                return Utils.CorrectResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
