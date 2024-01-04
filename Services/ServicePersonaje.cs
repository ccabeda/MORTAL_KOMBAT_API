using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Repository.IRepository;
using API_MortalKombat.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;
using MortalKombat_API.Controllers;
using MortalKombat_API.Models;
using MortalKombat_API.Models.DTOs.PersonajeDTO;
using System.Net;

namespace API_MortalKombat.Services
{
    public class ServicePersonaje : IServicePersonaje
    {
        private readonly IRepositoryPersonaje _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServicePersonaje> _logger;
        public ServicePersonaje(IMapper mapper, APIResponse apiresponse, ILogger<ServicePersonaje> logger, IRepositoryPersonaje repository)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _repository = repository;
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
                    _logger.LogError("El id " + id + "no esta registrado");
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
                    _logger.LogError("No se ingreso un nombre");
                    return _apiresponse;
                }
                var personaje = await _repository.ObtenerPorNombre(name);
                if (personaje == null)
                {
                    _apiresponse.isExit = false;
                    _apiresponse.statusCode = HttpStatusCode.NotFound;
                    _logger.LogError("El personaje " + name + " no esta registrado");
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
                _logger.LogError("Ocurrió un error al obtener la lista de personajes: " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }
        public async Task<APIResponse> CreatePersonaje([FromBody] PersonajeCreateDto personajeCreateDto)
        {
            try
            {
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
                _apiresponse.Result = personaje;
                _logger.LogInformation("¡Personaje creado con exito!");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al obtener la lista de villas: " + ex.Message);
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
                _apiresponse.Result = _mapper.Map<PersonajeDtoGetAll>(personaje);
                _logger.LogError("El personaje fue eliminado con exito.");
                return _apiresponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error al eliminar al perosnaje de id " + id + ": " + ex.Message);
                _apiresponse.isExit = false;
                _apiresponse.ErrorList = new List<string> { ex.ToString() }; //creo una lista que almacene el error
            }
            return _apiresponse;
        }

        public async Task<APIResponse> PutPersonaje(int id, [FromBody] PersonajeUpdateDto personajeUpdateDto)
        {
            try
            {
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
                _apiresponse.Result = existePersonaje;
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
    }
}
