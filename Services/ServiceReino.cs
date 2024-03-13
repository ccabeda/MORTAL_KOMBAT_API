using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ReinoDTO;
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
    public class ServiceReino : IServiceReino
    {
        private readonly IRepositoryReino _repository;
        private readonly IRepositoryPersonaje _repositoryPersonaje;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceReino> _logger;
        private readonly IValidator<ReinoCreateDto> _validator;
        private readonly IValidator<ReinoUpdateDto> _validatorUpdate;
        public ServiceReino(IMapper mapper, APIResponse apiresponse, ILogger<ServiceReino> logger, IRepositoryReino repository, IValidator<ReinoCreateDto> validator, 
            IValidator<ReinoUpdateDto> validatorUpdate, IRepositoryPersonaje repositoryPersonaje)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            _repositoryPersonaje = repositoryPersonaje;
        }

        public async Task<APIResponse> GetReinoById(int id)
        {
            try
            {
                var reino = await _repository.GetById(id);
                if (Utils.CheckIfNull(reino, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetReinoByName(string name)
        {
            try
            {
                var reino = await _repository.GetByName(name);
                if (Utils.CheckIfNull(reino, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetReinos()
        {
            try
            {
                List<Reino> listReinos = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<ReinoDto>>(listReinos);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateReino([FromBody] ReinoCreateDto reinoCreateDto)
        {
            try
            {
                if (await Utils.FluentValidator(reinoCreateDto, _validator, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var existReino = await _repository.GetByName(reinoCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existReino != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un personaje con el mismo nombre.");
                    return _apiresponse;
                }
                var reino = _mapper.Map<Reino>(reinoCreateDto);
                reino!.FechaCreacion = DateTime.Now;
                await _repository.Create(reino);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _logger.LogInformation("¡Reino creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }
    
        public async Task<APIResponse> DeleteReino(int id)
        {
            try 
            {
                var reino = await _repository.GetById(id); ;
                if (Utils.CheckIfNull(reino, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                var listPersonajes = await _repositoryPersonaje.GetAll();
                foreach (var i in listPersonajes) //aqui podria usarse el metodo cascada para que se borre todo, pero decidi agergarle esto para mas seguridad
                {
                    if (i.ClanId == id)
                    {
                        _apiresponse.statusCode = HttpStatusCode.NotFound;
                        _logger.LogError("El Reino no se puede eliminar porque el personaje " + i.Nombre + " de id " + i.Id + " contiene como ReinoId este reino.");
                        _apiresponse.isExit = false;
                        return _apiresponse;
                    }
                }
                await _repository.Delete(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ReinoDto>(reino);
                _logger.LogInformation("El reino fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdateReino(int id, [FromBody] ReinoUpdateDto reinoUpdateDto)
        {
            try
            {
                if (await Utils.FluentValidator(reinoUpdateDto, _validatorUpdate, _apiresponse, _logger) != null)
                {
                    return _apiresponse;
                }
                if (id == 0 || id != reinoUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existReino = await _repository.GetById(id);
                if (existReino == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(reinoUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != reinoUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un reino con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(reinoUpdateDto, existReino);
                existReino.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ReinoDto>(existReino);
                _logger.LogInformation("¡Reino Actualizado con exito!");
                await _repository.Update(existReino);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialReino(int id, JsonPatchDocument<ReinoUpdateDto> reinoUpdateDto)
        {
            try
            {
                var reino = await _repository.GetById(id);
                if (Utils.CheckIfNull(reino, _apiresponse, _logger) != null)
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
                if (registredName != null && registredName.Id != reino.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un reino con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(updateReinoDto, reino);
                reino.FechaActualizacion = DateTime.Now;
                await _repository.Update(reino);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = updateReinoDto;
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
