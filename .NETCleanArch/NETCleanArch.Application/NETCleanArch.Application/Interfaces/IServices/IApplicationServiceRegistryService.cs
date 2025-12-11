//using NETCleanArch.Application.Dtos;
//using NETCleanArch.Application.Dtos.Common;

//namespace NETCleanArch.Application.IServices
//{
//    public interface IApplicationServiceRegistryService
//    {
//        /// <summary>
//        /// GetApplicationServiceRegistryListService
//        /// </summary>
//        /// <returns></returns>
//        Task<ResponseModel<List<ApplicationServiceRegistryDto>>> GetApplicationServiceRegistryListService(PaginationRequest paginationRequest);

//        /// <summary>
//        /// AddApplicationServiceRegistryService
//        /// </summary>
//        /// <param name="applicationServiceRegistryDto"></param>
//        /// <returns></returns>
//        Task<ResponseModel<long>> AddApplicationServiceRegistryService(ApplicationServiceRegistryDto applicationServiceRegistryDto);

//        /// <summary>
//        /// GetApplicationServiceRegistryDetailService
//        /// </summary>
//        /// <param name="serviceregistryid"></param>
//        /// <returns></returns>
//        Task<ResponseModel<ApplicationServiceRegistryDto>> GetApplicationServiceRegistryDetailService(long serviceregistryid);

//        /// <summary>
//        /// DeleteApplicationServiceRegistryService
//        /// </summary>
//        /// <param name="serviceregistryid"></param>
//        /// <returns></returns>
//        Task<ResponseModel<bool>> DeleteApplicationServiceRegistryService(long serviceregistryid);

//        /// <summary>
//        /// InActiveApplicationServiceRegistryService
//        /// </summary>
//        /// <param name="serviceregistryid"></param>
//        /// <returns></returns>
//        Task<ResponseModel<bool>> InActiveApplicationServiceRegistryService(long serviceregistryid);

//    }
//}
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;

namespace NETCleanArch.Application.Interfaces.IServices
{
    public interface IApplicationServiceRegistryService
    {
        // Service Registry Operations
        Task<ResponseModel<List<ServiceRegistryDto>>> GetAllServiceRegistry(PaginationRequest paginationRequest);
        Task<ResponseModel<long>> AddServiceRegistry(ServiceRegistryDto serviceRegistryDto);
        Task<ResponseModel<ServiceRegistryDto>> GetServiceRegistryById(long serviceRegistryId);
        Task<ResponseModel<bool>> UpdateServiceRegistry(ServiceRegistryDto serviceRegistryDto);
        Task<ResponseModel<bool>> DeleteServiceRegistry(long serviceRegistryId);

        // Service Schema Operations
        Task<ResponseModel<List<ServiceSchemaDto>>> GetAllServiceSchemas(PaginationRequest paginationRequest);
        Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByApplicationId(long applicationId, PaginationRequest paginationRequest);
        Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByServiceId(long serviceId, PaginationRequest paginationRequest);
        Task<ResponseModel<long>> AddServiceSchema(ServiceSchemaDto serviceSchemaDto);
        Task<ResponseModel<ServiceSchemaDto>> GetServiceSchemaById(long schemaId);
        Task<ResponseModel<bool>> UpdateServiceSchema(ServiceSchemaDto serviceSchemaDto);
        Task<ResponseModel<bool>> DeleteServiceSchema(long schemaId);
    }
}