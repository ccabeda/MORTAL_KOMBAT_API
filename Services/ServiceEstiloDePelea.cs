using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Service
{
    public class ServiceEstiloDePelea : IServiceGeneric<EstiloDePeleaUpdateDto,EstiloDePeleaCreateDto>
    {
        private readonly IRepositoryGeneric<EstiloDePelea> _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceEstiloDePelea> _logger;
        public ServiceEstiloDePelea(IMapper mapper, APIResponse apiresponse, ILogger<ServiceEstiloDePelea> logger, IRepositoryGeneric<EstiloDePelea> repository)
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
                var estiloDePelea = await _repository.GetById(id);
                if (!Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
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
                var estiloDePelea = await _repository.GetByName(name);
                if (!Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.CorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
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
                List<EstiloDePelea> listEstilos = await _repository.GetAll();
                if (!Utils.CheckIfLsitIsNull<EstiloDePelea>(listEstilos, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                return Utils.ListCorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, listEstilos, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            try
            {
                var existEstilo = await _repository.GetByName(estiloCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfObjectExist<EstiloDePelea>(existEstilo, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var estiloDePelea = _mapper.Map<EstiloDePelea>(estiloCreateDto);
                estiloDePelea!.FechaCreacion = DateTime.Now;
                await _repository.Create(estiloDePelea);
                _logger.LogInformation("¡Estilo de pelea creado con exito!");
                return Utils.CorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
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
                var estiloDePelea = await _repository.GetById(id);
                if (!Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                await _repository.Delete(estiloDePelea);
                _logger.LogInformation("El clan fue eliminado con exito.");
                return Utils.CorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            try
            {
                var estiloDePelea = await _repository.GetById(estiloUpdateDto.Id);
                if (!Utils.CheckIfNull<EstiloDePelea>(estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(estiloUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<EstiloDePelea>(registredName, estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(estiloUpdateDto, estiloDePelea);
                estiloDePelea.FechaActualizacion = DateTime.Now;
                await _repository.Update(estiloDePelea);
                _logger.LogInformation("¡Estilo de pelea Actualizado con exito!");
                return Utils.CorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<EstiloDePeleaUpdateDto> estiloDePeleaUpdateDto)
        {
            try
            {
                var estiloDePelea = await _repository.GetById(id);
                if (!Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                var updateEstiloDePeleaDto = _mapper.Map<EstiloDePeleaUpdateDto>(estiloDePelea);
                estiloDePeleaUpdateDto.ApplyTo(updateEstiloDePeleaDto!);
                var registredName = await _repository.GetByName(updateEstiloDePeleaDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (!Utils.CheckIfNameAlreadyExist<EstiloDePelea>(registredName, estiloDePelea, _apiresponse, _logger))
                {
                    return _apiresponse;
                }
                _mapper.Map(updateEstiloDePeleaDto, estiloDePelea);
                estiloDePelea.FechaActualizacion = DateTime.Now;
                await _repository.Update(estiloDePelea);
                _logger.LogInformation("¡Estilo de pelea Actualizado con exito!");
                return Utils.CorrectResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
