using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServiceClan : IServiceGeneric<ClanUpdateDto, ClanCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceClan> _logger;
        public ServiceClan(IMapper mapper, APIResponse apiresponse, ILogger<ServiceClan> logger, IUnitOfWork unitOfWork)
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
                var clan = await _unitOfWork.repositoryClan.GetById(id);
                if (Utils.CheckIfNull(clan))
                {
                    _logger.LogError("El clan de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                var clan = await _unitOfWork.repositoryClan.GetByName(name);
                if (Utils.CheckIfNull(clan))
                {
                    _logger.LogError("El clan de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                List<Clan> listClanes = await _unitOfWork.repositoryClan.GetAll();
                if (Utils.CheckIfLsitIsNull<Clan>(listClanes))
                {
                    _logger.LogError("La lista de clanes esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<ClanDto, Clan>(_mapper, listClanes, _apiresponse);
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
                var existClan = await _unitOfWork.repositoryClan.GetByName(clanCreateDto.Nombre);
                if (Utils.CheckIfObjectExist<Clan>(existClan))
                {
                    _logger.LogError("El nombre del clan ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var clan = _mapper.Map<Clan>(clanCreateDto);
                clan!.FechaCreacion = DateTime.Now;
                await _unitOfWork.repositoryClan.Create(clan);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Clan creado con exito!");
                return Utils.OKResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                var clan = await _unitOfWork.repositoryClan.GetById(id);
                if (Utils.CheckIfNull(clan))
                {
                    _logger.LogError("El clan de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var listPersonajes = await _unitOfWork.repositoryPersonaje.GetAll();
                if (!Utils.PreventDeletionIfRelatedCharacterExist(clan ,listPersonajes, id))
                {
                    _logger.LogError("El clan no se puede eliminar porque hay un personaje que contiene como ClanId este clan.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                await _unitOfWork.repositoryClan.Delete(clan);
                await _unitOfWork.Save();
                _logger.LogInformation("El clan fue eliminado con exito.");
                return Utils.OKResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                var clan = await _unitOfWork.repositoryClan.GetById(clanUpdateDto.Id);
                if (Utils.CheckIfNull<Clan>(clan))
                {
                    _logger.LogError("El clan de id " + clanUpdateDto.Id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryClan.GetByName(clanUpdateDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Clan>(registredName, clan))
                {
                    _logger.LogError("El nombre del clan ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(clanUpdateDto, clan);
                clan.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryClan.Update(clan);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Clan Actualizado con exito!");
                return Utils.OKResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
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
                var clan = await _unitOfWork.repositoryClan.GetById(id);
                if (Utils.CheckIfNull(clan))
                {
                    _logger.LogError("El clan de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var updateClanDto = _mapper.Map<ClanUpdateDto>(clan); 
                clanUpdateDto.ApplyTo(updateClanDto!);
                var registredName = await _unitOfWork.repositoryClan.GetByName(updateClanDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Clan>(registredName, clan))
                {
                    _logger.LogError("El nombre del clan ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(updateClanDto, clan); 
                clan.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryClan.Update(clan);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Clan Actualizado con exito!");
                return Utils.OKResponse<ClanDto, Clan>(_mapper, clan, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
