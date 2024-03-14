using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using AutoMapper;
using FluentValidation;
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
        private readonly IValidator<ArmaCreateDto> _validator;
        private readonly IValidator<ArmaUpdateDto> _validatorUpdate;
        public ServiceArma(IMapper mapper, APIResponse apiresponse, ILogger<ServiceArma> logger, IRepositoryGeneric<Arma> repository, IValidator<ArmaCreateDto> validator,
            IValidator<ArmaUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
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
                Utils.CorrectResponse<ArmaDto,Arma>(_mapper, arma, _apiresponse);
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
                var arma = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
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
                List<Arma> listArmas = await _repository.GetAll();
                Utils.ListCorrectResponse<ArmaDto, Arma>(_mapper, listArmas, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Create([FromBody] ArmaCreateDto armaCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(armaCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    _logger.LogError("dsaudsagdas");
                    return _apiresponse;
                }
                var existArma = await _repository.GetByName(armaCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfObjectExist<Arma>(existArma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var arma = _mapper.Map<Arma>(armaCreateDto);
                arma!.FechaCreacion = DateTime.Now;
                await _repository.Create(arma);
                _logger.LogInformation("¡Arma creado con exito!");
                Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
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
                var arma = await _repository.GetById(id); ;
                if (!Utils.CheckIfNull(arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                await _repository.Delete(arma);
                _logger.LogInformation("El arma fue eliminado con exito.");
                Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Update([FromBody] ArmaUpdateDto armaUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(armaUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {

                    return _apiresponse;
                }
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
                Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex) 
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
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
                if (await Utils.FluentValidator(updateUserDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateUserDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Arma>(registredName, arma, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(updateUserDto, arma);
                arma.FechaActualizacion = DateTime.Now;
                await _repository.Update(arma);
                _logger.LogInformation("¡Arma Actualizado con exito!");
                Utils.CorrectResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    }
}
