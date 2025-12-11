using AutoMapper;
using Microsoft.Extensions.Logging;
using NETCleanArchApplication.Dtos;
using NETCleanArchApplication.Dtos.Common;
using NETCleanArchApplication.Interfaces.IRepositories;
using NETCleanArchApplication.Interfaces.IServices;
using NETCleanArchDomain.Entities;

namespace NETCleanArchApplication.Services
{
    public class ServiceRegistryService : IServiceRegistryService
    {
        #region Constructor
        private readonly IServiceRegistryRepository _serviceRegistryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceRegistryService> _logger;

        public ServiceRegistryService(
            IServiceRegistryRepository serviceRegistryRepository,
            IMapper mapper,
            ILogger<ServiceRegistryService> logger)
        {
            _serviceRegistryRepository = serviceRegistryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Service Registry Methods

        public async Task<ResponseModel<List<ServiceRegistryDto>>> GetAllServiceRegistry(PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetAllServiceRegistry(paginationRequest);
                if (result.IsSuccess)
                {
                    var dtoList = _mapper.Map<List<ServiceRegistryDto>>(result.Data);
                    return new ResponseModel<List<ServiceRegistryDto>>(true, result.Message, dtoList);
                }
                return new ResponseModel<List<ServiceRegistryDto>>(false, result.Message, new List<ServiceRegistryDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all service registries");
                return new ResponseModel<List<ServiceRegistryDto>>(false, AppCommon.ErrorMessage(), new List<ServiceRegistryDto>());
            }
        }

        public async Task<ResponseModel<long>> AddServiceRegistry(ServiceRegistryDto serviceRegistryDto)
        {
            try
            {
                var serviceRegistry = _mapper.Map<ServiceRegistry>(serviceRegistryDto);

                // Check if service registry already exists
                bool exists = await _serviceRegistryRepository.CheckServiceRegistryExists(
                    0, serviceRegistry.ServiceId, serviceRegistry.BaseUrl);

                if (exists)
                {
                    return new ResponseModel<long>(false, "Service registry with this service and base URL already exists", 0);
                }

                var result = await _serviceRegistryRepository.AddServiceRegistry(serviceRegistry);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding service registry");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        public async Task<ResponseModel<ServiceRegistryDto>> GetServiceRegistryById(long serviceRegistryId)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetServiceRegistryById(serviceRegistryId);
                if (result.IsSuccess)
                {
                    var dto = _mapper.Map<ServiceRegistryDto>(result.Data);
                    return new ResponseModel<ServiceRegistryDto>(true, result.Message, dto);
                }
                return new ResponseModel<ServiceRegistryDto>(false, result.Message, new ServiceRegistryDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting service registry by ID");
                return new ResponseModel<ServiceRegistryDto>(false, AppCommon.ErrorMessage(), new ServiceRegistryDto());
            }
        }

        public async Task<ResponseModel<bool>> UpdateServiceRegistry(ServiceRegistryDto serviceRegistryDto)
        {
            try
            {
                var serviceRegistry = _mapper.Map<ServiceRegistry>(serviceRegistryDto);

                // Check if another service registry exists with same service and base URL
                bool exists = await _serviceRegistryRepository.CheckServiceRegistryExists(
                    serviceRegistry.ServiceRegistryID, serviceRegistry.ServiceId, serviceRegistry.BaseUrl);

                if (exists)
                {
                    return new ResponseModel<bool>(false, "Another service registry with this service and base URL already exists", false);
                }

                var result = await _serviceRegistryRepository.UpdateServiceRegistry(serviceRegistry);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service registry");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<ResponseModel<bool>> DeleteServiceRegistry(long serviceRegistryId)
        {
            try
            {
                var result = await _serviceRegistryRepository.DeleteServiceRegistry(serviceRegistryId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service registry");
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
                if (result.IsSuccess)
                {
                    var dtoList = _mapper.Map<List<ServiceSchemaDto>>(result.Data);
                    return new ResponseModel<List<ServiceSchemaDto>>(true, result.Message, dtoList);
                }
                return new ResponseModel<List<ServiceSchemaDto>>(false, result.Message, new List<ServiceSchemaDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all service schemas");
                return new ResponseModel<List<ServiceSchemaDto>>(false, AppCommon.ErrorMessage(), new List<ServiceSchemaDto>());
            }
        }

        public async Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByApplicationId(long applicationId, PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetSchemasByApplicationId(applicationId, paginationRequest);
                if (result.IsSuccess)
                {
                    var dtoList = _mapper.Map<List<ServiceSchemaDto>>(result.Data);
                    return new ResponseModel<List<ServiceSchemaDto>>(true, result.Message, dtoList);
                }
                return new ResponseModel<List<ServiceSchemaDto>>(false, result.Message, new List<ServiceSchemaDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting schemas by application ID");
                return new ResponseModel<List<ServiceSchemaDto>>(false, AppCommon.ErrorMessage(), new List<ServiceSchemaDto>());
            }
        }

        public async Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByServiceId(long serviceId, PaginationRequest paginationRequest)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetSchemasByServiceId(serviceId, paginationRequest);
                if (result.IsSuccess)
                {
                    var dtoList = _mapper.Map<List<ServiceSchemaDto>>(result.Data);
                    return new ResponseModel<List<ServiceSchemaDto>>(true, result.Message, dtoList);
                }
                return new ResponseModel<List<ServiceSchemaDto>>(false, result.Message, new List<ServiceSchemaDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting schemas by service ID");
                return new ResponseModel<List<ServiceSchemaDto>>(false, AppCommon.ErrorMessage(), new List<ServiceSchemaDto>());
            }
        }

        public async Task<ResponseModel<long>> AddServiceSchema(ServiceSchemaDto serviceSchemaDto)
        {
            try
            {
                var serviceSchema = _mapper.Map<ServiceSchema>(serviceSchemaDto);

                // Check if schema already exists
                bool exists = await _serviceRegistryRepository.CheckSchemaExists(
                    0, serviceSchema.ApplicationId, serviceSchema.ServiceId, serviceSchema.Endpoint);

                if (exists)
                {
                    return new ResponseModel<long>(false, "Schema with this application, service, and endpoint already exists", 0);
                }

                var result = await _serviceRegistryRepository.AddServiceSchema(serviceSchema);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding service schema");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        public async Task<ResponseModel<ServiceSchemaDto>> GetServiceSchemaById(long schemaId)
        {
            try
            {
                var result = await _serviceRegistryRepository.GetServiceSchemaById(schemaId);
                if (result.IsSuccess)
                {
                    var dto = _mapper.Map<ServiceSchemaDto>(result.Data);
                    return new ResponseModel<ServiceSchemaDto>(true, result.Message, dto);
                }
                return new ResponseModel<ServiceSchemaDto>(false, result.Message, new ServiceSchemaDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting service schema by ID");
                return new ResponseModel<ServiceSchemaDto>(false, AppCommon.ErrorMessage(), new ServiceSchemaDto());
            }
        }

        public async Task<ResponseModel<bool>> UpdateServiceSchema(ServiceSchemaDto serviceSchemaDto)
        {
            try
            {
                var serviceSchema = _mapper.Map<ServiceSchema>(serviceSchemaDto);

                // Check if another schema exists with same application, service, and endpoint
                bool exists = await _serviceRegistryRepository.CheckSchemaExists(
                    serviceSchema.SchemaId, serviceSchema.ApplicationId, serviceSchema.ServiceId, serviceSchema.Endpoint);

                if (exists)
                {
                    return new ResponseModel<bool>(false, "Another schema with this application, service, and endpoint already exists", false);
                }

                var result = await _serviceRegistryRepository.UpdateServiceSchema(serviceSchema);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service schema");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<ResponseModel<bool>> DeleteServiceSchema(long schemaId)
        {
            try
            {
                var result = await _serviceRegistryRepository.DeleteServiceSchema(schemaId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service schema");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        #endregion
    }
}
