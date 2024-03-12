using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service.IService;
using API_MortalKombat.Services.Utils;
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
                var arma = await _repository.GetById(id);
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
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetArmaByName(string name)
        {
            try
            {
                var arma = await _repository.GetByName(name);
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
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetArmas()
        {
            try
            {
                IEnumerable<Arma> listArmas = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<ArmaDto>>(listArmas);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Armas: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateArma([FromBody] ArmaCreateDto armaCreateDto)
        {
            try
            {
                if (Utils.FluentValidator(armaCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existArma = await _repository.GetByName(armaCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existArma != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un arma con el mismo nombre.");
                    return _apiresponse;
                }
                var arma = _mapper.Map<Arma>(armaCreateDto);
                arma!.FechaCreacion = DateTime.Now;
                await _repository.Create(arma);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<ArmaDto>(arma);
                _logger.LogInformation("¡Arma creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el arma: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeleteArma(int id)
        {
            try
            {
                var arma = await _repository.GetById(id); ;
                if (arma == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El arma no se encuentra registrada. Verifica que el id ingresado sea correcto.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Delete(arma);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ArmaDto>(arma);
                _logger.LogInformation("El arma fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar el arma de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateArma(int id, [FromBody] ArmaUpdateDto armaUpdateDto)
        {
            try
            {
                if (Utils.FluentValidator(armaUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (id == 0 || id != armaUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existArma = await _repository.GetById(id);
                if (existArma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(armaUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != armaUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un arma con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(armaUpdateDto, existArma);
                existArma.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ArmaDto>(existArma);
                _logger.LogInformation("¡Arma Actualizado con exito!");
                await _repository.Update(existArma);
                return _apiresponse;
            }
            catch (Exception ex) 
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el arma: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialArma(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            try
            {
                var arma = await _repository.GetById(id);
                if (arma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                        return _apiresponse;
                }
                var updateUserDto = _mapper.Map<ArmaUpdateDto>(arma); 
                armaUpdateDto.ApplyTo(updateUserDto!);
                if (Utils.FluentValidator(updateUserDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _mapper.Map(updateUserDto, arma);
                arma.FechaActualizacion = DateTime.Now;
                await _repository.Update(arma); 
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = updateUserDto;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el arma de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; 
            }
            return _apiresponse;
        }
    }
}
