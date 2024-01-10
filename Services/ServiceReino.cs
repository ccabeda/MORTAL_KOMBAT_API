using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IServices;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;
using MortalKombat_API.Models;
using MortalKombat_API.Models.DTOs.ReinoDTO;
using System.Net;

namespace API_MortalKombat.Services
{
    public class ServiceReino : IServiceReino
    {
        private readonly IRepositoryReino _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServicePersonaje> _logger;
        private readonly IValidator<ReinoCreateDto> _validator;
        private readonly IValidator<ReinoUpdateDto> _validatorUpdate;
        public ServiceReino(IMapper mapper, APIResponse apiresponse, ILogger<ServicePersonaje> logger, IRepositoryReino repository, IValidator<ReinoCreateDto> validator, 
            IValidator<ReinoUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            
        }
        public async Task<APIResponse> GetReinoById(int id)
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
                var reino = await _repository.ObtenerPorId(id);
                if (reino == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id " + id + "no esta registrado");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el reino de id: " + id + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetReinoByName(string name)
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
                var reino = await _repository.ObtenerPorNombre(name);
                if (reino == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El reino " + name + " no esta registrado");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el reino de nombre: " + name + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetReinos()
        {
            try
            {
                IEnumerable<Reino> lista_reinos = await _repository.ObtenerTodos();
                _apiresponse.Result = _mapper.Map<IEnumerable<ReinoDto>>(lista_reinos);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de reinos: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateReino([FromBody] ReinoCreateDto reinoCreateDto)
        {
            try
            {
                var fluent_validation = await _validator.ValidateAsync(reinoCreateDto); //uso de fluent validations

                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (reinoCreateDto == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var existereino = await _repository.ObtenerPorNombre(reinoCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existereino != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un personaje con el mismo nombre.");
                    return _apiresponse;
                }
                var reino = _mapper.Map<Reino>(reinoCreateDto);
                reino.FechaCreacion = DateTime.Now;
                await _repository.Crear(reino);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = reino;
                _logger.LogInformation("¡Personaje creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el reino: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    
        public async Task<APIResponse> DeleteReino(int id)
        {
            try 
            {
                if (id == 0)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _apiresponse.isExit = false;
                    _logger.LogError("Error al encontrar el reino.");
                    return _apiresponse;
                }
                var reino = await _repository.ObtenerPorId(id); ;
                if (reino == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El reino no se encuentra registrado.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Eliminar(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _logger.LogError("El reino fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar el reino de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateReino(int id, [FromBody] ReinoUpdateDto reinoUpdateDto)
        {
            try
            {
                var fluent_validation = await _validatorUpdate.ValidateAsync(reinoUpdateDto); //uso de fluent validations

                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (id == 0 || id != reinoUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existereino = await _repository.ObtenerPorId(id);
                if (existereino == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                _mapper.Map(reinoUpdateDto, existereino);
                existereino.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = existereino;
                _logger.LogInformation("¡Reino Actualizado con exito!");
                await _repository.Actualizar(existereino);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el reino: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    }
}
