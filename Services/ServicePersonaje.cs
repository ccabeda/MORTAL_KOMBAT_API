using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Service.IService;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_MortalKombat.Service
{
    public class ServicePersonaje : IServicePersonaje
    {
        private readonly IRepositoryPersonaje _repository;
        private readonly IRepositoryArma _repositoryArma;
        private readonly IRepositoryEstiloDePelea _repositoryEstiloDePelea;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServicePersonaje> _logger;
        private readonly IValidator<PersonajeCreateDto> _validator;
        private readonly IValidator<PersonajeUpdateDto> _validatorUpdate;
        public ServicePersonaje(IMapper mapper, APIResponse apiresponse, ILogger<ServicePersonaje> logger, IRepositoryPersonaje repository, IValidator<PersonajeCreateDto> validator, 
                                IValidator<PersonajeUpdateDto> validatorUpdate, IRepositoryArma repositoryArma, IRepositoryEstiloDePelea repositoryEstiloDePelea)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            _repositoryArma = repositoryArma;
            _repositoryEstiloDePelea = repositoryEstiloDePelea;
        }

        public async Task<APIResponse> GetPersonajeById(int id)
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
                var personaje = await _repository.ObtenerPorId(id);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id " + id + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener al personaje de id: " + id + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetPersonajeByName(string name)
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
                var personaje = await _repository.ObtenerPorNombre(name);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El personaje " + name + " no esta registrado.");
                    return _apiresponse;
                }
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener al personaje de nombre: " + name + " : " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> GetPersonajes()
        {
            try
            {
                IEnumerable<Personaje> lista_personajes = await _repository.ObtenerTodos();
                _apiresponse.Result = _mapper.Map<IEnumerable<PersonajeDtoGetAll>>(lista_personajes);
                _apiresponse.statusCode = HttpStatusCode.OK;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de Personajes: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> CreatePersonaje([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            try
            {
                var fluent_validation = await _validator.ValidateAsync(personajeCreateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                    if (personajeCreateDto == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El id 0 no se puede utilizar.");
                    return _apiresponse;
                }
                var existepersonaje = await _repository.ObtenerPorNombre(personajeCreateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (existepersonaje != null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.Conflict; // Conflict indica que ya existe un recurso con el mismo nombre
                    _logger.LogError("Ya existe un personaje con el mismo nombre.");
                    return _apiresponse;
                }
                var personaje = _mapper.Map<Personaje>(personajeCreateDto);
                personaje.FechaCreacion = DateTime.Now;
                await _repository.Crear(personaje);
                _apiresponse.statusCode = HttpStatusCode.Created;
                _apiresponse.Result = _mapper.Map<PersonajeUpdateDto>(personaje); //para mostrar los datos que quiero y no tener que crear otro Dto, uso update.
                _logger.LogInformation("¡Personaje creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar crear el Personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> DeletePersonaje(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _apiresponse.isExit = false;
                    _logger.LogError("Error al encontrar el personaje.");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorId(id); ;
                if (personaje == null)
                {
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El personaje no se encuentra registrado.");
                    _apiresponse.isExit = false;
                    return _apiresponse;
                }
                await _repository.Eliminar(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("El personaje fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar el personaje de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePersonaje(int id, [FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            try
            {
                var fluent_validation = await _validatorUpdate.ValidateAsync(personajeUpdateDto); //uso de fluent validations
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;
                }
                if (id == 0 || id != personajeUpdateDto.Id)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con la id ingresada.");
                    return _apiresponse;
                }
                var existePersonaje = await _repository.ObtenerPorId(id);
                if (existePersonaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("No se encuentra registrado el id ingresado.");
                    return _apiresponse;
                }
                _mapper.Map(personajeUpdateDto, existePersonaje);
                existePersonaje.FechaActualizacion = DateTime.Now;
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = _mapper.Map<PersonajeDto>(existePersonaje);
                _logger.LogInformation("¡Personaje Actualizado con exito!");
                await _repository.Actualizar(existePersonaje);
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar al personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> AddWeaponToPersonaje(int id_personaje, int id_arma)
        {
            try
            {
                if (id_personaje == 0 || id_arma ==0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var arma = await _repositoryArma.ObtenerPorId(id_arma);
                if (arma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del arma ingresada.");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorId(id_personaje);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }                
                personaje.Armas.Add(arma);
                await _repository.Guardar();
                _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("Arma agregada con exito a personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = personaje;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al inetntar agregar un arma al personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> RemoveWeaponToPersonaje(int id_personaje, int id_arma)
        {
            try
            {
                if (id_personaje == 0 || id_arma == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var arma = await _repositoryArma.ObtenerPorId(id_arma);
                if (arma == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del arma ingresada.");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorId(id_personaje);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                if (!personaje.Armas.Any(arma => arma.Id == id_arma))
                    {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El personaje no cuenta con ese arma.");
                    return _apiresponse;
                }
                personaje.Armas.Remove(arma);
                await _repository.Guardar();
                _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("Arma removida con exito del personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = personaje;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar agregar un arma al personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> AddStyleToPersonaje(int id_personaje, int id_estilo_de_pelea)
        {
            try
            {
                if (id_personaje == 0 || id_estilo_de_pelea == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var estilo = await _repositoryEstiloDePelea.ObtenerPorId(id_estilo_de_pelea);
                if (estilo == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del estilo de pelea ingresada.");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorId(id_personaje);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                personaje.EstilosDePeleas.Add(estilo);
                await _repository.Guardar();
                _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("Arma agregada con exito a personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = personaje;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al inetntar agregar un estilo de pelea al personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> RemoveStyleToPersonaje(int id_personaje, int id_estilo_de_pelea)
        {
            try
            {
                if (id_personaje == 0 || id_estilo_de_pelea == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error, no se puede ingresar un id = 0.");
                    return _apiresponse;
                }
                var estilo = await _repositoryEstiloDePelea.ObtenerPorId(id_estilo_de_pelea);
                if (estilo == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del estilo de pelea ingresada.");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorId(id_personaje);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("Error con el id del personaje ingresada.");
                    return _apiresponse;
                }
                if (!personaje.EstilosDePeleas.Any(estilo => estilo.Id == id_estilo_de_pelea))
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El personaje no cuenta con ese estilo de pelea.");
                    return _apiresponse;
                }
                personaje.EstilosDePeleas.Remove(estilo);
                await _repository.Guardar();
                _mapper.Map<PersonajeDto>(personaje);
                _logger.LogInformation("Estilo de pelea removido con exito del personaje.");
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = personaje;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar agregar un estilo de pelea al personaje: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> UpdatePartialPersonaje(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto)
        {
            try
            {
                if (personajeUpdateDto == null || id == 0)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Error al ingresar los datos.");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorId(id);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("El id ingresado no esta registrado");
                    return _apiresponse;
                }
                var personajeDTO = _mapper.Map<PersonajeUpdateDto>(personaje);
                personajeUpdateDto.ApplyTo(personajeDTO);
                var fluent_validation = await _validatorUpdate.ValidateAsync(personajeDTO);
                if (!fluent_validation.IsValid)
                {
                    var errors = fluent_validation.Errors.Select(error => error.ErrorMessage).ToList();
                    _logger.LogError("Error al validar los datos de entrada.");
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.BadRequest;
                    _apiresponse.ErrorList = errors;
                    return _apiresponse;

                }
                _mapper.Map(personajeDTO, personaje);
                personaje.FechaActualizacion = DateTime.Now;
                await _repository.Actualizar(personaje);
                _apiresponse.statusCode = HttpStatusCode.OK;
                _apiresponse.Result = personajeDTO;
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al intentar actualizar el personaje de id: " + id + ". Error: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() };
            }
            return _apiresponse;
        }
    }
}
