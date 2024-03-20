using API_MortalKombat.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using Microsoft.AspNetCore.JsonPatch;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;

namespace API_MortalKombat.Service
{
    public class ServiceEstiloDePelea : IServiceGeneric<EstiloDePeleaUpdateDto,EstiloDePeleaCreateDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceEstiloDePelea> _logger;
        public ServiceEstiloDePelea(IMapper mapper, APIResponse apiresponse, ILogger<ServiceEstiloDePelea> logger, IUnitOfWork unitOfWork)
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
                var estiloDePelea = await _unitOfWork.repositoryEstiloDePelea.GetById(id);
                if (Utils.CheckIfNull(estiloDePelea))
                {
                    _logger.LogError("El estilo de pelea de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
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
                var estiloDePelea = await _unitOfWork.repositoryEstiloDePelea.GetByName(name);
                if (Utils.CheckIfNull(estiloDePelea))
                {
                    _logger.LogError("El estilo de pelea de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
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
                List<EstiloDePelea> listEstilos = await _unitOfWork.repositoryEstiloDePelea.GetAll();
                if (Utils.CheckIfLsitIsNull<EstiloDePelea>(listEstilos))
                {
                    _logger.LogError("La lista de estilos de pelea esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, listEstilos, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] EstiloDePeleaCreateDto estiloCreateDto)
        {
            try
            {
                var existEstilo = await _unitOfWork.repositoryEstiloDePelea.GetByName(estiloCreateDto.Nombre);
                if (!Utils.CheckIfNull<EstiloDePelea>(existEstilo))
                {
                    _logger.LogError("El nombre del estilo de pelea ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var estiloDePelea = _mapper.Map<EstiloDePelea>(estiloCreateDto);
                estiloDePelea!.FechaCreacion = DateTime.Now;
                await _unitOfWork.repositoryEstiloDePelea.Create(estiloDePelea);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Estilo de pelea creado con exito!");
                return Utils.OKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
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
                var estiloDePelea = await _unitOfWork.repositoryEstiloDePelea.GetById(id);
                if (Utils.CheckIfNull(estiloDePelea))
                {
                    _logger.LogError("El estilo de pelea de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                await _unitOfWork.repositoryEstiloDePelea.Delete(estiloDePelea);
                await _unitOfWork.Save();
                _logger.LogInformation("El clan fue eliminado con exito.");
                return Utils.OKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] EstiloDePeleaUpdateDto estiloUpdateDto)
        {
            try
            {
                var estiloDePelea = await _unitOfWork.repositoryEstiloDePelea.GetById(estiloUpdateDto.Id);
                if (Utils.CheckIfNull<EstiloDePelea>(estiloDePelea))
                {
                    _logger.LogError("El estilo de pelea de id " + estiloUpdateDto.Id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryEstiloDePelea.GetByName(estiloUpdateDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<EstiloDePelea>(registredName, estiloDePelea))
                {
                    _logger.LogError("El nombre del estilo de pelea ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(estiloUpdateDto, estiloDePelea);
                estiloDePelea.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryEstiloDePelea.Update(estiloDePelea);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Estilo de pelea Actualizado con exito!");
                return Utils.OKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<EstiloDePeleaUpdateDto> estiloDePeleaUpdateDto)
        {
            try
            {
                var estiloDePelea = await _unitOfWork.repositoryEstiloDePelea.GetById(id);
                if (Utils.CheckIfNull(estiloDePelea))
                {
                    _logger.LogError("El estilo de pelea de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var updateEstiloDePeleaDto = _mapper.Map<EstiloDePeleaUpdateDto>(estiloDePelea);
                estiloDePeleaUpdateDto.ApplyTo(updateEstiloDePeleaDto!);
                var registredName = await _unitOfWork.repositoryEstiloDePelea.GetByName(updateEstiloDePeleaDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<EstiloDePelea>(registredName, estiloDePelea))
                {
                    _logger.LogError("El nombre del estilo de pelea ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(updateEstiloDePeleaDto, estiloDePelea);
                estiloDePelea.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryEstiloDePelea.Update(estiloDePelea);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Estilo de pelea Actualizado con exito!");
                return Utils.OKResponse<EstiloDePeleaDto, EstiloDePelea>(_mapper, estiloDePelea, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
