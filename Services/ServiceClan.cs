using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service.IService;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Service
{
    public class ServiceClan : IServiceClan
    {
        private readonly IRepositoryClan _repository;
        private readonly IRepositoryPersonaje _repositoryPersonaje;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceClan> _logger;
        private readonly IValidator<ClanCreateDto> _validator;
        private readonly IValidator<ClanUpdateDto> _validatorUpdate;
        public ServiceClan(IMapper mapper, APIResponse apiresponse, ILogger<ServiceClan> logger, IRepositoryClan repository, IValidator<ClanCreateDto> validator, 
            IValidator<ClanUpdateDto> validatorUpdate, IRepositoryPersonaje repositoryPersonaje)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;   
            _repositoryPersonaje = repositoryPersonaje;
        }

        public async Task<APIResponse> GetClanById(int id)
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
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el clan de id: " + id + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetClanByName(string name)
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
                var clan = await _repository.GetByName(name);
                if (clan == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El clan " + name + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener el clan de nombre: " + name + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetClanes()
        {
            try
            {
                IEnumerable<Clan> listClanes = await _repository.GetAll();
                _apiresponse.Result = _mapper.Map<IEnumerable<ClanDto>>(listClanes);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Clanes: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreateClan([FromBody] ClanCreateDto clanCreateDto)
        {
            try
            {
                var fluentValidation = await _validator.ValidateAsync(clanCreateDto); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                var existClan = await _repository.GetByName(clanCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existClan != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                var clan = _mapper.Map<Clan>(clanCreateDto);
                clan!.FechaCreacion = DateTime.Now;
                await _repository.Create(clan);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
                _logger.LogInformation("¡Clan creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el clan: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
    
        public async Task<APIResponse> DeleteClan(int id)
        {
            try
            {
                var clan = await _repository.GetById(id); ;
                if (clan == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El clan no se encuentra registrado. Verifica que el id ingresado sea correcto.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                var listCharacter = await _repositoryPersonaje.GetAll();
                foreach (var i in listCharacter) //aqui podria usarse el metodo cascada para que se borre todo, pero decidi agergarle esto para mas seguridad
                {
                    if (i.ClanId == id )
                    {
                        _apiresponse.statusCode = HttpStatusCode.NotFound;
                        _logger.LogError("El clan no se puede eliminar porque el personaje "+ i.Nombre+ " de id "+ i.Id +" contiene como ClanId este clan.");
                        _apiresponse.isExit = false;
                        return _apiresponse;
                    }
                }
                await _repository.Delete(clan);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ClanDto>(clan);
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

        public async Task<APIResponse> UpdateClan(int id, [FromBody] ClanUpdateDto clanUpdateDto)
        {
            try
            {
                var fluentValidation = await _validatorUpdate.ValidateAsync(clanUpdateDto); //uso de fluent validations
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (id == 0 || id != clanUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existClan = await _repository.GetById(id);
                if (existClan == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                var registredName = await _repository.GetByName(clanUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (registredName != null && registredName.Id != clanUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un clan con el mismo nombre.");
                    return _apiresponse;
                }
                _mapper.Map(clanUpdateDto, existClan);
                existClan.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<ClanDto>(existClan);
                _logger.LogInformation("¡Clan Actualizado con exito!");
                await _repository.Update(existClan);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el clan: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialClan(int id, JsonPatchDocument<ClanUpdateDto> clanUpdateDto)
        {
            try
            {
                var clan = await _repository.GetById(id);
                if (clan == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                    return _apiresponse;
                }
                var clanDTO = _mapper.Map<ClanUpdateDto>(clan); 
                clanUpdateDto.ApplyTo(clanDTO!); 
                var fluentValidation = await _validatorUpdate.ValidateAsync(clanDTO!); 
                if (!fluentValidation.IsValid)
                {
                    var errors = fluentValidation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;

                }
                _mapper.Map(clanDTO, clan); 
                clan.FechaActualizacion = DateTime.Now;
                await _repository.Update(clan); 
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = clanDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el clan de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.statusCode = HttpStatusCode.NotFound;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; 
            }
            return _apiresponse;
        }
    }
}
