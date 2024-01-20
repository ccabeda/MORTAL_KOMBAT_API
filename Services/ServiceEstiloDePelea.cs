using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;

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
                if (id == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var clan = await _repository.ObtenerPorId(id);
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
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetEstiloDePeleaByName(string name)
        {
            try
            {
                if (name == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se ingreso un nombre.");
                    return _apiresponse;
                }
                var clan = await _repository.ObtenerPorNombre(name);
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
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetEstilosDePelea()
        {
            try
            {
                IEnumerable<EstiloDePelea> lista_estilos = await _repository.ObtenerTodos();
                _apiresponse.Result = _mapper.Map<IEnumerable<EstiloDePeleaDto>>(lista_estilos);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Estilos De Pelea: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            try
            {
                var fluent_validation = await _validator.ValidateAsync(estiloCreateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (estiloCreateDto == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var existeestilo = await _repository.ObtenerPorNombre(estiloCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existeestilo != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                var estilo = _mapper.Map<EstiloDePelea>(estiloCreateDto);
                estilo.FechaCreacion = DateTime.Now;
                await _repository.Crear(estilo);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = estilo;
                _logger.LogInformation("¡Estilo de pelea creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el estilo de pelea: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteEstiloDePelea(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _apiresponse.isExit = false;
                    _logger.LogError("Error al encontrar el estilo de pelea.");
                    return _apiresponse;
                }
                var estilo = await _repository.ObtenerPorId(id); ;
                if (estilo == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El estilo de pelea no se encuentra registrado.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Eliminar(estilo);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<EstiloDePeleaDto>(estilo);
                _logger.LogInformation("El clan fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar el clan de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateEstiloDePelea(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            try
            {
                var fluent_validation = await _validatorUpdate.ValidateAsync(estiloUpdateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
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
                var existeestilo = await _repository.ObtenerPorId(id);
                if (existeestilo == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                _mapper.Map(estiloUpdateDto, existeestilo);
                existeestilo.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = existeestilo;
                _logger.LogInformation("¡Estilo de pelea Actualizado con exito!");
                await _repository.Actualizar(existeestilo);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el estilo de pelea: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    }
}
