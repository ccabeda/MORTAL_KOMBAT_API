using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServiceReino : IServiceGeneric<ReinoUpdateDto, ReinoCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceReino> _logger;
        public ServiceReino(IMapper mapper, APIResponse apiresponse, ILogger<ServiceReino> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _apiresponse = apiresponse;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                var reino = await _unitOfWork.repositoryReino.GetById(id);
                if (Utils.CheckIfNull(reino))
                {
                    _logger.LogError("El reino de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> GetByName(string name)
        {
            try
            {
                var reino = await _unitOfWork.repositoryReino.GetByName(name);
                if (Utils.CheckIfNull(reino))
                {
                    _logger.LogError("El reino de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> GetAll()
        {
            try
            {
                List<Reino> listReinos = await _unitOfWork.repositoryReino.GetAll();
                if (Utils.CheckIfLsitIsNull<Reino>(listReinos))
                {
                    _logger.LogError("La lista de reinos esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<ReinoDto, Reino>(_mapper, listReinos, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] ReinoCreateDto reinoCreateDto)
        {
            try
            {
                var existReino = await _unitOfWork.repositoryReino.GetByName(reinoCreateDto.Nombre);
                if (!Utils.CheckIfNull<Reino>(existReino))
                {
                    _logger.LogError("El nombre del reino ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var reino = _mapper.Map<Reino>(reinoCreateDto);
                reino!.FechaCreacion = DateTime.Now;
                await _unitOfWork.repositoryReino.Create(reino);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Reino creado con exito!");
                return Utils.OKResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    
        public async Task<APIResponse> Delete(int id)
        {
            try 
            {
                var reino = await _unitOfWork.repositoryReino.GetById(id);
                if (Utils.CheckIfNull(reino))
                {
                    _logger.LogError("El reino de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var listPersonajes = await _unitOfWork.repositoryPersonaje.GetAll();
                if (!Utils.PreventDeletionIfRelatedCharacterExist(reino, listPersonajes, id))
                {
                    _logger.LogError("El reino no se puede eliminar porque hay un personaje que contiene como ReinoId este reino.");
                    return Utils.BadRequestResponse(_apiresponse);
                }
                await _unitOfWork.repositoryReino.Delete(reino);
                await _unitOfWork.Save();
                _logger.LogInformation("El reino fue eliminado con exito.");
                return Utils.OKResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] ReinoUpdateDto reinoUpdateDto)
        {
            try
            {
                var reino = await _unitOfWork.repositoryReino.GetById(reinoUpdateDto.Id);
                if (Utils.CheckIfNull<Reino>(reino))
                {
                    _logger.LogError("El reino de id " + reinoUpdateDto.Id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryReino.GetByName(reinoUpdateDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Reino>(registredName, reino))
                {
                    _logger.LogError("El nombre del reino ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(reinoUpdateDto, reino);
                reino.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryReino.Update(reino);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Reino Actualizado con exito!");
                return Utils.OKResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<ReinoUpdateDto> reinoUpdateDto)
        {
            try
            {
                var reino = await _unitOfWork.repositoryReino.GetById(id);
                if (Utils.CheckIfNull(reino))
                {
                    _logger.LogError("El reino de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var updateReinoDto = _mapper.Map<ReinoUpdateDto>(reino);
                reinoUpdateDto.ApplyTo(updateReinoDto!);
                var registredName = await _unitOfWork.repositoryReino.GetByName(updateReinoDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Reino>(registredName, reino))
                {
                    _logger.LogError("El nombre del reino ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(updateReinoDto, reino);
                reino.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryReino.Update(reino);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Reino Actualizado con exito!");
                return Utils.OKResponse<ReinoDto, Reino>(_mapper, reino, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
