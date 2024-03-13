using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Services.Utils;

namespace API_MortalKombat.Service
{
    public class ServiceEstiloDePelea : IServiceEstiloDePelea
    {
        private readonly IRepositoryEstiloDePelea _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceEstiloDePelea> _logger;
        private readonly IValidator<EstiloDePeleaCreateDto> _validator;
        private readonly IValidator<EstiloDePeleaUpdateDto> _validatorUpdate;
        public ServiceEstiloDePelea(IMapper mapper, APIResponse apiresponse, ILogger<ServiceEstiloDePelea> logger, IRepositoryEstiloDePelea repository,
            IValidator<EstiloDePeleaCreateDto> validator, IValidator<EstiloDePeleaUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetEstiloDePeleaById(int id)
        {
            try
            {
                var estiloDePelea = await _repository.GetById(id);
                if (Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(estiloDePelea);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetEstiloDePeleaByName(string name)
        {
            try
            {
                var estiloDePelea = await _repository.GetByName(name);
                if (Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(estiloDePelea);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetEstilosDePelea()
        {
            try
            {
                List<EstiloDePelea> listEstilos = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<EstiloDePeleaDto>>(listEstilos);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(estiloCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existStyle = await _repository.GetByName(estiloCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existStyle != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                var estilo = _mapper.Map<EstiloDePelea>(estiloCreateDto);
                estilo!.FechaCreacion = DateTime.Now;
                await _repository.Create(estilo);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(estilo);
                _logger.LogInformation("¡Estilo de pelea creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteEstiloDePelea(int id)
        {
            try
            {
                var estiloDePelea = await _repository.GetById(id);
                if (Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                await _repository.Delete(estiloDePelea);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(estiloDePelea);
                _logger.LogInformation("El clan fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateEstiloDePelea(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(estiloUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (id == 0 || id != estiloUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existStyle = await _repository.GetById(id);
                if (existStyle == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(estiloUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != estiloUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un estilo de pelea con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(estiloUpdateDto, existStyle);
                existStyle.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(existStyle);
                _logger.LogInformation("¡Estilo de pelea Actualizado con exito!");
                await _repository.Update(existStyle);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialEstiloDePelea(int id, JsonPatchDocument<EstiloDePeleaUpdateDto> estiloDePeleaUpdateDto)
        {
            try
            {
                var estiloDePelea = await _repository.GetById(id);
                if (Utils.CheckIfNull(estiloDePelea, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var updateEstiloDePeleaDto = _mapper.Map<EstiloDePeleaUpdateDto>(estiloDePelea);
                estiloDePeleaUpdateDto.ApplyTo(updateEstiloDePeleaDto!);
                if (await Utils.FluentValidator(updateEstiloDePeleaDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(updateEstiloDePeleaDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != estiloDePelea.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un estilo de pelea con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(updateEstiloDePeleaDto, estiloDePelea);
                estiloDePelea.FechaActualizacion = DateTime.Now;
                await _repository.Update(estiloDePelea);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = updateEstiloDePeleaDto;
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
