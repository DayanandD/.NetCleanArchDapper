using NETCleanArchApplication.Dtos.Common;
using NETCleanArchDomain.Entities;

namespace NETCleanArchApplication.Interfaces.IRepositories
{
    public interface IServiceRegistryRepository
    {
        // Service Registry Operations
        Task<ResponseModel<List<ServiceRegistry>>> GetAllServiceRegistry(PaginationRequest paginationRequest);
        Task<ResponseModel<long>> AddServiceRegistry(ServiceRegistry serviceRegistry);
        Task<ResponseModel<ServiceRegistry>> GetServiceRegistryById(long serviceRegistryId);
        Task<ResponseModel<bool>> UpdateServiceRegistry(ServiceRegistry serviceRegistry);
        Task<ResponseModel<bool>> DeleteServiceRegistry(long serviceRegistryId);
        Task<bool> CheckServiceRegistryExists(long serviceRegistryId, long? serviceId, string baseUrl);

        // Service Schema Operations
        Task<ResponseModel<List<ServiceSchema>>> GetAllServiceSchemas(PaginationRequest paginationRequest);
        Task<ResponseModel<List<ServiceSchema>>> GetSchemasByApplicationId(long applicationId, PaginationRequest paginationRequest);
        Task<ResponseModel<List<ServiceSchema>>> GetSchemasByServiceId(long serviceId, PaginationRequest paginationRequest);
        Task<ResponseModel<long>> AddServiceSchema(ServiceSchema serviceSchema);
        Task<ResponseModel<ServiceSchema>> GetServiceSchemaById(long schemaId);
        Task<ResponseModel<bool>> UpdateServiceSchema(ServiceSchema serviceSchema);
        Task<ResponseModel<bool>> DeleteServiceSchema(long schemaId);
        Task<bool> CheckSchemaExists(long schemaId, long applicationId, long serviceId, string endpoint);
    }
}
