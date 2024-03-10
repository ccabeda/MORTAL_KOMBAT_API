using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.JsonPatch;

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
                var clan = await _repository.GetById(id);
                if (clan == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id " + id + "no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el estilo de pelea de id: " + id + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetEstiloDePeleaByName(string name)
        {
            try
            {
                var clan = await _repository.GetByName(name);
                if (clan == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El estilo de pelea " + name + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el estilo de pelea de nombre: " + name + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetEstilosDePelea()
        {
            try
            {
                IEnumerable<EstiloDePelea> listEstilos = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<EstiloDePeleaDto>>(listEstilos);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Estilos De Pelea: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(estiloCreateDto); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
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
                _logger.LogError("Ocurrió un error al intentar crear el estilo de pelea: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteEstiloDePelea(int id)
        {
            try
            {
                var estilo = await _repository.GetById(id);
                if (estilo == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El estilo de pelea no se encuentra registrado. Verifica que el id ingresado sea correcto.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Delete(estilo);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(estilo);
                _logger.LogInformation("El clan fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar el clan de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateEstiloDePelea(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(estiloUpdateDto); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
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
                _logger.LogError("Ocurrió un error al intentar actualizar el estilo de pelea: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialEstiloDePelea(int id, JsonPatchDocument<EstiloDePeleaUpdateDto> estiloDePeleaUpdateDto)
        {
            try
            {
                var estiloDePelea = await _repository.GetById(id);
                if (estiloDePelea == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                    return _apiresponse;
                }
                var estiloDePeleaDTO = _mapper.Map<EstiloDePeleaUpdateDto>(estiloDePelea);
                estiloDePeleaUpdateDto.ApplyTo(estiloDePeleaDTO!);
                var fluentValidation = await _validatorUpdate.ValidateAsync(estiloDePeleaDTO!);
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;

                }
                _mapper.Map(estiloDePeleaDTO, estiloDePelea);
                estiloDePelea.FechaActualizacion = DateTime.Now;
                await _repository.Update(estiloDePelea);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = estiloDePeleaDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el estilo de pelea de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }
    }
}
