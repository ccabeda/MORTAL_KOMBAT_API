using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service.IService;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Service
{
    public class ServiceArma : IServiceArma
    {
        private readonly IRepositoryArma _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceArma> _logger;
        private readonly IValidator<ArmaCreateDto> _validator;
        private readonly IValidator<ArmaUpdateDto> _validatorUpdate;
        public ServiceArma(IMapper mapper, APIResponse apiresponse, ILogger<ServiceArma> logger, IRepositoryArma repository, IValidator<ArmaCreateDto> validator,
            IValidator<ArmaUpdateDto> validatorUpdate)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }

        public async Task<APIResponse> GetArmaById(int id)
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
                var arma = await _repository.ObtenerPorId(id);
                if (arma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id " + id + "no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ArmaDto>(arma);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el arma de id: " + id + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetArmaByName(string name)
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
                var arma = await _repository.ObtenerPorNombre(name);
                if (arma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El arma " + name + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ArmaDto>(arma);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el arma de nombre: " + name + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetArmas()
        {
            try
            {
                IEnumerable<Arma> lista_armas = await _repository.ObtenerTodos();
                _apiresponse.Result = _mapper.Map<IEnumerable<ArmaDto>>(lista_armas);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Armas: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateArma([FromBody] ArmaCreateDto armaCreateDto)
        {
            try
            {
                var fluent_validation = await _validator.ValidateAsync(armaCreateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (armaCreateDto == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var existearma = await _repository.ObtenerPorNombre(armaCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existearma != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un arma con el mismo nombre.");
                    return _apiresponse;
                }
                var arma = _mapper.Map<Arma>(armaCreateDto);
                arma.FechaCreacion = DateTime.Now;
                await _repository.Crear(arma);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<ArmaDto>(arma);
                _logger.LogInformation("¡Arma creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el arma: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteArma(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _apiresponse.isExit = false;
                    _logger.LogError("Error al encontrar el arma.");
                    return _apiresponse;
                }
                var arma = await _repository.ObtenerPorId(id); ;
                if (arma == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El arma no se encuentra registrado.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Eliminar(arma);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ArmaDto>(arma);
                _logger.LogInformation("El arma fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar el arma de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateArma(int id, [FromBody] ArmaUpdateDto armaUpdateDto)
        {
            try
            {
                var fluent_validation = await _validatorUpdate.ValidateAsync(armaUpdateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (id == 0 || id != armaUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existearma = await _repository.ObtenerPorId(id);
                if (existearma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                _mapper.Map(armaUpdateDto, existearma);
                existearma.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ArmaDto>(existearma);
                _logger.LogInformation("¡Arma Actualizado con exito!");
                await _repository.Actualizar(existearma);
                return _apiresponse;
            }
            catch (Exception ex) 
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el arma: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialArma(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            try
            {
                if (armaUpdateDto == null || id == 0) //verifico que la id no sea 0 o que el json sea null
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Error al ingresar los datos.");
                    return _apiresponse;
                }
                var arma = await _repository.ObtenerPorId(id);
                if (arma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                        return _apiresponse;
                }
                var armaDTO = _mapper.Map<ArmaUpdateDto>(arma); 
                armaUpdateDto.ApplyTo(armaDTO); 
                var fluent_validation = await _validatorUpdate.ValidateAsync(armaDTO);
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;

                }
                _mapper.Map(armaDTO, arma);
                arma.FechaActualizacion = DateTime.Now;
                await _repository.Actualizar(arma); 
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = armaDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el arma de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; 
            }
            return _apiresponse;
        }
    }
}
