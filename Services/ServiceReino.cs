using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServiceReino : IServiceGeneric<ReinoUpdateDto, ReinoCreateDto>
    {
        private readonly IRepositoryGeneric<Reino> _repository;
        private readonly IRepositoryGeneric<Personaje> _repositoryPersonaje;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceReino> _logger;
        private readonly IValidator<ReinoCreateDto> _validator;
        private readonly IValidator<ReinoUpdateDto> _validatorUpdate;
        public ServiceReino(IMapper mapper, APIResponse apiresponse, ILogger<ServiceReino> logger, IRepositoryGeneric<Reino> repository, IValidator<ReinoCreateDto> validator, 
            IValidator<ReinoUpdateDto> validatorUpdate, IRepositoryGeneric<Personaje> repositoryPersonaje)
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
                var reino = await _repository.GetById(id);
                if (!Utils.CheckIfNull(reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                Utils.CorrectResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
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
                var reino = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                Utils.CorrectResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
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
                List<Reino> listReinos = await _repository.GetAll();
                Utils.ListCorrectResponse<ReinoDto, Reino>(_mapper, listReinos, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Create([FromBody] ReinoCreateDto reinoCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(reinoCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existReino = await _repository.GetByName(reinoCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfObjectExist<Reino>(existReino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var reino = _mapper.Map<Reino>(reinoCreateDto);
                reino!.FechaCreacion = DateTime.Now;
                await _repository.Create(reino);
                _logger.LogInformation("¡Reino creado con exito!");
                Utils.CorrectResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
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
                var reino = await _repository.GetById(id); ;
                if (!Utils.CheckIfNull(reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var listPersonajes = await _repositoryPersonaje.GetAll();
                if (!Utils.PreventDeletionIfRelaredCharacterExist(listPersonajes, _apiresponse, id))
                {
                    _logger.LogError("El reino no se puede eliminar porque hay un personaje que contiene como ReinoId este reino.");
                    return _apiresponse;
                }
                await _repository.Delete(reino);
                _logger.LogInformation("El reino fue eliminado con exito.");
                Utils.CorrectResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> Update([FromBody] ReinoUpdateDto reinoUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(reinoUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var reino = await _repository.GetById(reinoUpdateDto.Id);
                if (!Utils.CheckIfNull<Reino>(reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(reinoUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Reino>(registredName, reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(reinoUpdateDto, reino);
                reino.FechaActualizacion = DateTime.Now;
                await _repository.Update(reino);
                _logger.LogInformation("¡Reino Actualizado con exito!");
                Utils.CorrectResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<ReinoUpdateDto> reinoUpdateDto)
        {
            try
            {
                var reino = await _repository.GetById(id);
                if (!Utils.CheckIfNull(reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var updateReinoDto = _mapper.Map<ReinoUpdateDto>(reino);
                reinoUpdateDto.ApplyTo(updateReinoDto!);
                if (await Utils.FluentValidator(updateReinoDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateReinoDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<Reino>(registredName, reino, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(updateReinoDto, reino);
                reino.FechaActualizacion = DateTime.Now;
                await _repository.Update(reino);
                _logger.LogInformation("¡Reino Actualizado con exito!");
                Utils.CorrectResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    }
}
