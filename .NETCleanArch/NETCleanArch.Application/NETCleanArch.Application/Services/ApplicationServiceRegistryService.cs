//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using System.Data;
//using NETCleanArch.Application.Dtos;
//using NETCleanArch.Application.Interfaces.IRepositories;
//using NETCleanArch.Application.Interfaces.IServices;
//using NETCleanArch.Application.Dtos.Common;
//
//namespace NETCleanArch.Application.Services
//{
//    public class ApplicationServiceRegistryService : IApplicationServiceRegistryService
//    {
//        private readonly IApplicationServiceRegistryRepository _applicationServiceRegistryRepository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<ApplicationServiceRegistryService> _logger;

//        #region Constructor
//        public ApplicationServiceRegistryService(
//            IApplicationServiceRegistryRepository applicationServiceRegistryRepository,
//            IMapper mapper,
//            ILogger<ApplicationServiceRegistryService> logger)
//        {
//            _applicationServiceRegistryRepository = applicationServiceRegistryRepository;
//            _mapper = mapper;
//            _logger = logger;
//        }
//        #endregion

//        #region Method
//        /// <summary>
//        /// GetApplicationServiceRegistryListService
//        /// </summary>
//        /// <returns></returns>
//        public async Task<ResponseModel<List<ApplicationServiceRegistryDto>>> GetApplicationServiceRegistryListService(PaginationRequest paginationRequest)
//        {
//            try
//            {
//                var data = await _applicationServiceRegistryRepository.GetApplicationServiceRegistryListRepository(paginationRequest);
//                if (data.IsSuccess)
//                {
//                    var dtoList = _mapper.Map<List<ApplicationServiceRegistryDto>>(data.Data);
//                    return new ResponseModel<List<ApplicationServiceRegistryDto>>(true, AppCommon.GetAllMessage("Application Service Registry"), dtoList);
//                }
//                else
//                    return new ResponseModel<List<ApplicationServiceRegistryDto>>(data.IsSuccess, data.Message, new List<ApplicationServiceRegistryDto>());
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "");
//                return new ResponseModel<List<ApplicationServiceRegistryDto>>(false, AppCommon.ErrorMessage(), new List<ApplicationServiceRegistryDto>());
//            }
//        }

//        /// <summary>
//        /// AddApplicationServiceRegistryService
//        /// </summary>
//        /// <param name="applicationServiceRegistryDto"></param>
//        /// <returns></returns>
//        public async Task<ResponseModel<long>> AddApplicationServiceRegistryService(ApplicationServiceRegistryDto applicationServiceRegistryDto)
//        {
//            try
//            {
//                bool isvalid = await _applicationServiceRegistryRepository.CheckApplicationServiceRegistryRepository(applicationServiceRegistryDto);
//                if (!isvalid)
//                {
//                    var response = await _applicationServiceRegistryRepository.AddApplicationServiceRegistryRepository(applicationServiceRegistryDto);
//                    return response;
//                }
//                else
//                    return new ResponseModel<long>(false, AppCommon.AlreadyExistsMessage("Application Service Registry"), 0);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "");
//                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
//            }
//        }

//        /// <summary>
//        /// GetApplicationServiceRegistryDetailService         
//        /// </summary>
//        /// <param name="serviceregistryid"></param>
//        /// <returns></returns>
//        public async Task<ResponseModel<ApplicationServiceRegistryDto>> GetApplicationServiceRegistryDetailService(long serviceregistryid)
//        {
//            try
//            {
//                var data = await _applicationServiceRegistryRepository.GetApplicationServiceRegistryDetailRepository(serviceregistryid);
//                if (data.IsSuccess)
//                {
//                    var dto = _mapper.Map<ApplicationServiceRegistryDto>(data.Data);
//                    return new ResponseModel<ApplicationServiceRegistryDto>(true, AppCommon.GetMessage("Application Service Registry"), dto);
//                }
//                else
//                    return new ResponseModel<ApplicationServiceRegistryDto>(data.IsSuccess, data.Message, new ApplicationServiceRegistryDto());
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "");
//                return new ResponseModel<ApplicationServiceRegistryDto>(false, AppCommon.ErrorMessage(), new ApplicationServiceRegistryDto());
//            }
//        }

//        /// <summary>
//        /// DeleteApplicationServiceRegistryService
//        /// </summary>
//        /// <param name="serviceregistryid"></param>
//        /// <returns></returns>
//        public async Task<ResponseModel<bool>> DeleteApplicationServiceRegistryService(long serviceregistryid)
//        {
//            using var connection = CreateConnection();
//            connection.Open();
//            using var transaction = connection.BeginTransaction();
//            try
//            {
//                var deleteRegistry = await _applicationServiceRegistryRepository.DeleteApplicationServiceRegistryRepository(serviceregistryid, connection, transaction);
//                if (!deleteRegistry.IsSuccess)
//                {
//                    transaction.Rollback();
//                    return deleteRegistry;
//                }

//                transaction.Commit();
//                return new ResponseModel<bool>(true, AppCommon.DeleteMessage("Application Service Registry"), true);
//            }
//            catch (Exception ex)
//            {
//                transaction.Rollback();
//                _logger.LogError(ex, "");
//                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
//            }
//        }

//        /// <summary>
//        /// InActiveApplicationServiceRegistryService
//        /// </summary>
//        /// <param name="serviceregistryid"></param>
//        /// <returns></returns>
//        public async Task<ResponseModel<bool>> InActiveApplicationServiceRegistryService(long serviceregistryid)
//        {
//            using var connection = CreateConnection();
//            connection.Open();
//            using var transaction = connection.BeginTransaction();
//            try
//            {
//                var inactiveRegistry = await _applicationServiceRegistryRepository.InActiveApplicationServiceRegistryRepository(serviceregistryid, connection, transaction);
//                if (!inactiveRegistry.IsSuccess)
//                {
//                    transaction.Rollback();
//                    return inactiveRegistry;
//                }

//                transaction.Commit();
//                return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Application Service Registry"), true);
//            }
//            catch (Exception ex)
//            {
//                transaction.Rollback();
//                _logger.LogError(ex, "");
//                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
//            }
//        }

//        private IDbConnection CreateConnection()
//        {
//            throw new NotImplementedException();
//        }


//        #endregion
//    }
//}


using Microsoft.Extensions.Logging;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;
using NETCleanArch.Application.Interfaces.IRepositories;
using NETCleanArch.Application.Interfaces.IServices;
using NETCleanArchDomain.Entities;

namespace NETCleanArch.Application.Services
{
    public class ApplicationServiceRegistryService : IApplicationServiceRegistryService
    {
        private readonly IServiceRegistryRepository _serviceRegistryRepository;
        private readonly ILogger<ApplicationServiceRegistryService> _logger;

        public ApplicationServiceRegistryService(
            IServiceRegistryRepository serviceRegistryRepository,
            ILogger<ApplicationServiceRegistryService> logger)
        {
            _serviceRegistryRepository = serviceRegistryRepository;
            _logger = logger;
        }

        #region Service Registry Methods

        public async Task<ResponseModel<List<ServiceRegistryDto>>> GetAllServiceRegistry(PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetAllServiceRegistry(paginationRequest);

                if (!result.IsSuccess)
                    return new ResponseModel<List<ServiceRegistryDto>>(false, result.Message, new List<ServiceRegistryDto>());

                var dtoList = result.Data.Select(MapToDto).ToList();
                return new ResponseModel<List<ServiceRegistryDto>>(true, result.Message, dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllServiceRegistry service");
                return new ResponseModel<List<ServiceRegistryDto>>(false, AppCommon.ErrorMessage(), new List<ServiceRegistryDto>());
            }
        }

        public async Task<ResponseModel<long>> AddServiceRegistry(ServiceRegistryDto serviceRegistryDto)
        {
            try
            {
                var entity = MapToEntity(serviceRegistryDto);
                return await _serviceRegistryRepository.AddServiceRegistry(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddServiceRegistry service");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        public async Task<ResponseModel<ServiceRegistryDto>> GetServiceRegistryById(long serviceRegistryId)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetServiceRegistryById(serviceRegistryId);

                if (!result.IsSuccess)
                    return new ResponseModel<ServiceRegistryDto>(false, result.Message, new ServiceRegistryDto());

                var dto = MapToDto(result.Data);
                return new ResponseModel<ServiceRegistryDto>(true, result.Message, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceRegistryById service");
                return new ResponseModel<ServiceRegistryDto>(false, AppCommon.ErrorMessage(), new ServiceRegistryDto());
            }
        }

        public async Task<ResponseModel<bool>> UpdateServiceRegistry(ServiceRegistryDto serviceRegistryDto)
        {
            try
            {
                var entity = MapToEntity(serviceRegistryDto);
                return await _serviceRegistryRepository.UpdateServiceRegistry(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateServiceRegistry service");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<ResponseModel<bool>> DeleteServiceRegistry(long serviceRegistryId)
        {
            try
            {
                return await _serviceRegistryRepository.DeleteServiceRegistry(serviceRegistryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceRegistry service");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        #endregion

        #region Service Schema Methods

        public async Task<ResponseModel<List<ServiceSchemaDto>>> GetAllServiceSchemas(PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetAllServiceSchemas(paginationRequest);

                if (!result.IsSuccess)
                    return new ResponseModel<List<ServiceSchemaDto>>(false, result.Message, new List<ServiceSchemaDto>());

                var dtoList = result.Data.Select(MapToDto).ToList();
                return new ResponseModel<List<ServiceSchemaDto>>(true, result.Message, dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllServiceSchemas service");
                return new ResponseModel<List<ServiceSchemaDto>>(false, AppCommon.ErrorMessage(), new List<ServiceSchemaDto>());
            }
        }

        public async Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByApplicationId(long applicationId, PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetSchemasByApplicationId(applicationId, paginationRequest);

                if (!result.IsSuccess)
                    return new ResponseModel<List<ServiceSchemaDto>>(false, result.Message, new List<ServiceSchemaDto>());

                var dtoList = result.Data.Select(MapToDto).ToList();
                return new ResponseModel<List<ServiceSchemaDto>>(true, result.Message, dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSchemasByApplicationId service");
                return new ResponseModel<List<ServiceSchemaDto>>(false, AppCommon.ErrorMessage(), new List<ServiceSchemaDto>());
            }
        }

        public async Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByServiceId(long serviceId, PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetSchemasByServiceId(serviceId, paginationRequest);

                if (!result.IsSuccess)
                    return new ResponseModel<List<ServiceSchemaDto>>(false, result.Message, new List<ServiceSchemaDto>());

                var dtoList = result.Data.Select(MapToDto).ToList();
                return new ResponseModel<List<ServiceSchemaDto>>(true, result.Message, dtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSchemasByServiceId service");
                return new ResponseModel<List<ServiceSchemaDto>>(false, AppCommon.ErrorMessage(), new List<ServiceSchemaDto>());
            }
        }

        public async Task<ResponseModel<long>> AddServiceSchema(ServiceSchemaDto serviceSchemaDto)
        {
            try
            {
                var entity = MapToEntity(serviceSchemaDto);
                return await _serviceRegistryRepository.AddServiceSchema(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddServiceSchema service");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        public async Task<ResponseModel<ServiceSchemaDto>> GetServiceSchemaById(long schemaId)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetServiceSchemaById(schemaId);

                if (!result.IsSuccess)
                    return new ResponseModel<ServiceSchemaDto>(false, result.Message, new ServiceSchemaDto());

                var dto = MapToDto(result.Data);
                return new ResponseModel<ServiceSchemaDto>(true, result.Message, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceSchemaById service");
                return new ResponseModel<ServiceSchemaDto>(false, AppCommon.ErrorMessage(), new ServiceSchemaDto());
            }
        }

        public async Task<ResponseModel<bool>> UpdateServiceSchema(ServiceSchemaDto serviceSchemaDto)
        {
            try
            {
                var entity = MapToEntity(serviceSchemaDto);
                return await _serviceRegistryRepository.UpdateServiceSchema(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateServiceSchema service");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<ResponseModel<bool>> DeleteServiceSchema(long schemaId)
        {
            try
            {
                return await _serviceRegistryRepository.DeleteServiceSchema(schemaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceSchema service");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        #endregion

        #region Mapping Methods

        private ServiceRegistryDto MapToDto(ServiceRegistry entity)
        {
            if (entity == null) return null;

            return new ServiceRegistryDto
            {
                ServiceRegistryId = entity.ServiceRegistryID,
                ServiceId = entity.ServiceId,
                ServiceName = entity.ServiceName,
                BaseUrl = entity.BaseUrl,
                AddEndpoint = entity.AddEndpoint,
                UpdateEndpoint = entity.UpdateEndpoint,
                DeleteEndpoint = entity.DeleteEndpoint,
                AddonEndpoint = entity.AddonEndpoint,
                IsActive = entity.IsActive,
                RequiresAuth = entity.RequiresAuth,
                TimeoutSeconds = entity.TimeoutSeconds,
                RetryCount = entity.RetryCount,
                OperationContext = entity.OperationContext,
                PayloadTemplate = entity.PayloadTemplate
            };
        }

        private ServiceRegistry MapToEntity(ServiceRegistryDto dto)
        {
            return new ServiceRegistry
            {
                ServiceRegistryID = dto.ServiceRegistryId,
                ServiceId = (long)dto.ServiceId,
                ServiceName = dto.ServiceName,
                BaseUrl = dto.BaseUrl,
                AddEndpoint = dto.AddEndpoint,
                UpdateEndpoint = dto.UpdateEndpoint,
                DeleteEndpoint = dto.DeleteEndpoint,
                AddonEndpoint = dto.AddonEndpoint,
                IsActive = dto.IsActive,
                RequiresAuth = dto.RequiresAuth,
                TimeoutSeconds = dto.TimeoutSeconds,
                RetryCount = dto.RetryCount,
                OperationContext = dto.OperationContext,
                PayloadTemplate = dto.PayloadTemplate
            };
        }

        private ServiceSchemaDto MapToDto(ServiceSchema entity)
        {
            if (entity == null) return null;

            return new ServiceSchemaDto
            {
                SchemaId = entity.SchemaId,
                ApplicationId = entity.ApplicationId,
                ServiceId = entity.ServiceId,
                Endpoint = entity.Endpoint,
                Method = entity.Method,
                SchemaJson = entity.SchemaJson,
                Version = entity.Version,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                ApplicationName = entity.ApplicationName,
                ServiceName = entity.ServiceName
            };
        }

        private ServiceSchema MapToEntity(ServiceSchemaDto dto)
        {
            return new ServiceSchema
            {
                SchemaId = dto.SchemaId,
                ApplicationId = dto.ApplicationId,
                ServiceId = dto.ServiceId,
                Endpoint = dto.Endpoint,
                Method = dto.Method,
                SchemaJson = dto.SchemaJson,
                Version = dto.Version,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedOn = dto.CreatedOn,
                UpdatedBy = dto.UpdatedBy,
                UpdatedOn = dto.UpdatedOn
            };
        }

        #endregion
    }
}