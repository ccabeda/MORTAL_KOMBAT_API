using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
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
    public class ServiceClan : IServiceClan
    {
        private readonly IRepositoryClan _repository;
        private readonly IRepositoryPersonaje _repositoryPersonaje;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceClan> _logger;
        private readonly IValidator<ClanCreateDto> _validator;
        private readonly IValidator<ClanUpdateDto> _validatorUpdate;
        public ServiceClan(IMapper mapper, APIResponse apiresponse, ILogger<ServiceClan> logger, IRepositoryClan repository, IValidator<ClanCreateDto> validator, 
            IValidator<ClanUpdateDto> validatorUpdate, IRepositoryPersonaje repositoryPersonaje)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;   
            _repositoryPersonaje = repositoryPersonaje;
        }

        public async Task<APIResponse> GetClanById(int id)
        {
            try
            {
                var clan = await _repository.GetById(id);
                if (Utils.CheckIfNull(clan, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetClanByName(string name)
        {
            try
            {
                var clan = await _repository.GetByName(name);
                if (Utils.CheckIfNull(clan, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetClanes()
        {
            try
            {
                List<Clan> listClanes = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<ClanDto>>(listClanes);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateClan([FromBody] ClanCreateDto clanCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(clanCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existClan = await _repository.GetByName(clanCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existClan != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                var clan = _mapper.Map<Clan>(clanCreateDto);
                clan!.FechaCreacion = DateTime.Now;
                await _repository.Create(clan);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _logger.LogInformation("¡Clan creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    
        public async Task<APIResponse> DeleteClan(int id)
        {
            try
            {
                var clan = await _repository.GetById(id); ;
                if (Utils.CheckIfNull(clan, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var listCharacter = await _repositoryPersonaje.GetAll();
                foreach (var i in listCharacter) //aqui podria usarse el metodo cascada para que se borre todo, pero decidi agergarle esto para mas seguridad
                {
                    if (i.ClanId == id )
                    {
                        _apiresponse.statusCode = HttpStatusCode.NotFound;
                        _logger.LogError("El clan no se puede eliminar porque el personaje "+ i.Nombre+ " de id "+ i.Id +" contiene como ClanId este clan.");
                        _apiresponse.isExit = false;
                        return _apiresponse;
                    }
                }
                await _repository.Delete(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _logger.LogInformation("El clan fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateClan(int id, [FromBody] ClanUpdateDto clanUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(clanUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (id == 0 || id != clanUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existClan = await _repository.GetById(id);
                if (existClan == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(clanUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != clanUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(clanUpdateDto, existClan);
                existClan.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ClanDto>(existClan);
                _logger.LogInformation("¡Clan Actualizado con exito!");
                await _repository.Update(existClan);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialClan(int id, JsonPatchDocument<ClanUpdateDto> clanUpdateDto)
        {
            try
            {
                var clan = await _repository.GetById(id);
                if (Utils.CheckIfNull(clan, _apiresponse, _logger) != null)
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
                if (registredName != null && registredName.Id != clan.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(updateClanDto, clan); 
                clan.FechaActualizacion = DateTime.Now;
                await _repository.Update(clan); 
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = updateClanDto;
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
