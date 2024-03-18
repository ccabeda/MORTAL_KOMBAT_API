using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServiceArma : IServiceGeneric<ArmaUpdateDto, ArmaCreateDto>
    {
        private readonly IRepositoryGeneric<Arma> _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceArma> _logger;
        public ServiceArma(IMapper mapper, APIResponse apiresponse, ILogger<ServiceArma> logger, IRepositoryGeneric<Arma> repository)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var arma = await _repository.GetById(id);
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<ArmaDto,Arma>(_mapper, arma, _apiresponse);
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
                var arma = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
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
                List<Arma> listArmas = await _repository.GetAll();
                if (!Utils.CheckIfLsitIsNull<Arma>(listArmas,_apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.ListCorrectResponse<ArmaDto, Arma>(_mapper, listArmas, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] ArmaCreateDto armaCreateDto)
        {
            try
            {
                var existArma = await _repository.GetByName(armaCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfObjectExist<Arma>(existArma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var arma = _mapper.Map<Arma>(armaCreateDto);
                arma!.FechaCreacion = DateTime.Now;
                await _repository.Create(arma);
                _logger.LogInformation("¡Arma creado con exito!");
                return Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
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
                var arma = await _repository.GetById(id); ;
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                await _repository.Delete(arma);
                _logger.LogInformation("El arma fue eliminado con exito.");
                return Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] ArmaUpdateDto armaUpdateDto)
        {
            try
            {
                var arma = await _repository.GetById(armaUpdateDto.Id);
                if (!Utils.CheckIfNull<Arma>(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(armaUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Arma>(registredName,arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(armaUpdateDto, arma);
                arma.FechaActualizacion = DateTime.Now; 
                await _repository.Update(arma);
                _logger.LogInformation("¡Arma Actualizado con exito!");
                return Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex) 
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            try
            {
                var arma = await _repository.GetById(id);
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var updateUserDto = _mapper.Map<ArmaUpdateDto>(arma); 
                armaUpdateDto.ApplyTo(updateUserDto!);
                var registredName = await _repository.GetByName(updateUserDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Arma>(registredName, arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(updateUserDto, arma);
                arma.FechaActualizacion = DateTime.Now;
                await _repository.Update(arma);
                _logger.LogInformation("¡Arma Actualizado con exito!");
                return Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
