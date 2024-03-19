using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Services.IService;
using API_MortalKombat.Services.Utils;
using API_MortalKombat.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service
{
    public class ServiceArma : IServiceGeneric<ArmaUpdateDto, ArmaCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiresponse;
        private readonly ILogger<ServiceArma> _logger;
        public ServiceArma(IMapper mapper, APIResponse apiresponse, ILogger<ServiceArma> logger, IUnitOfWork unitOfWork)
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
                var arma = await _unitOfWork.repositoryArma.GetById(id);
                if (Utils.CheckIfNull(arma))
                {
                    _logger.LogError("El arma de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<ArmaDto,Arma>(_mapper, arma, _apiresponse);
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
                var arma = await _unitOfWork.repositoryArma.GetByName(name);
                if (Utils.CheckIfNull(arma))
                {
                    _logger.LogError("El arma de nombre " + name + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.OKResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
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
                List<Arma> listArmas = await _unitOfWork.repositoryArma.GetAll();
                if (Utils.CheckIfLsitIsNull<Arma>(listArmas))
                {
                    _logger.LogError("La lista de armas esta vacia.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                return Utils.ListOKResponse<ArmaDto, Arma>(_mapper, listArmas, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Create([FromBody] ArmaCreateDto armaCreateDto)
        {
            try
            {
                var existArma = await _unitOfWork.repositoryArma.GetByName(armaCreateDto.Nombre);  //verifico que no haya otro con el mismo nombre
                if (Utils.CheckIfObjectExist<Arma>(existArma))
                {
                    _logger.LogError("El nombre del arma ya se encuentra registrado.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                var arma = _mapper.Map<Arma>(armaCreateDto);
                arma!.FechaCreacion = DateTime.Now;
                await _unitOfWork.repositoryArma.Create(arma);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Arma creado con exito!");
                return Utils.OKResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
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
                var arma = await _unitOfWork.repositoryArma.GetById(id);
                if (Utils.CheckIfNull(arma))
                {
                    _logger.LogError("El arma de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                await _unitOfWork.repositoryArma.Delete(arma);
                await _unitOfWork.Save();
                _logger.LogInformation("El arma fue eliminado con exito.");
                return Utils.OKResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> Update([FromBody] ArmaUpdateDto armaUpdateDto)
        {
            try
            {
                var arma = await _unitOfWork.repositoryArma.GetById(armaUpdateDto.Id);
                if (Utils.CheckIfNull<Arma>(arma))
                {
                    _logger.LogError("El arma de id " + armaUpdateDto.Id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var registredName = await _unitOfWork.repositoryArma.GetByName(armaUpdateDto.Nombre); //verifico que no haya otro con el mismo nomrbe
                if (Utils.CheckIfNameAlreadyExist<Arma>(registredName,arma))
                {
                    _logger.LogError("El nombre del arma ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(armaUpdateDto, arma);
                arma.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryArma.Update(arma);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Arma Actualizado con exito!");
                return Utils.OKResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex) 
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }

        public async Task<APIResponse> UpdatePartial(int id, JsonPatchDocument<ArmaUpdateDto> armaUpdateDto)
        {
            try
            {
                var arma = await _unitOfWork.repositoryArma.GetById(id);
                if (Utils.CheckIfNull(arma))
                {
                    _logger.LogError("El arma de id " + id + " no esta registrado.");
                    return Utils.NotFoundResponse(_apiresponse);
                }
                var updateUserDto = _mapper.Map<ArmaUpdateDto>(arma); 
                armaUpdateDto.ApplyTo(updateUserDto!);
                var registredName = await _unitOfWork.repositoryArma.GetByName(updateUserDto.Nombre);
                if (Utils.CheckIfNameAlreadyExist<Arma>(registredName, arma))
                {
                    _logger.LogError("El nombre del arma ya se encuentra registrado. Por favor, utiliza otro.");
                    return Utils.ConflictResponse(_apiresponse);
                }
                _mapper.Map(updateUserDto, arma);
                arma.FechaActualizacion = DateTime.Now;
                await _unitOfWork.repositoryArma.Update(arma);
                await _unitOfWork.Save();
                _logger.LogInformation("¡Arma Actualizado con exito!");
                return Utils.OKResponse<ArmaDto, Arma>(_mapper, arma, _apiresponse);
            }
            catch (Exception ex)
            {
                return Utils.ErrorHandling(ex, _apiresponse, _logger);
            }
        }
    }
}
