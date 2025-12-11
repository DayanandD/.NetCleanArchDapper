using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;

namespace NETCleanArch.Application.IServices
{
    public interface IApplicationServiceRegistrySchemaService
    {
        /// <summary>
        /// GetApplicationServiceRegistrySchemaListService
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetApplicationServiceRegistrySchemaListService(PaginationRequest paginationRequest);

        /// <summary>
        /// AddApplicationServiceRegistrySchemaService
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        Task<ResponseModel<long>> AddApplicationServiceRegistrySchemaService(ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto);

        /// <summary>
        /// GetApplicationServiceRegistrySchemaDetailService
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        Task<ResponseModel<ApplicationServiceRegistrySchemaDto>> GetApplicationServiceRegistrySchemaDetailService(long schemasid);

        /// <summary>
        /// DeleteApplicationServiceRegistrySchemaService
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        Task<ResponseModel<bool>> DeleteApplicationServiceRegistrySchemaService(long schemasid);

        /// <summary>
        /// InActiveApplicationServiceRegistrySchemaService
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        Task<ResponseModel<bool>> InActiveApplicationServiceRegistrySchemaService(long schemasid);

        /// <summary>
        /// GetSchemasByApplicationServiceService
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetSchemasByApplicationServiceService(long applicationid, long serviceid);
    }
}