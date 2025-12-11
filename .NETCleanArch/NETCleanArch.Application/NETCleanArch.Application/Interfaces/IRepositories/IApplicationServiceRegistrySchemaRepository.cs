using System.Data;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;

namespace NETCleanArch.Application.Interfaces.IRepositories
{
    public interface IApplicationServiceRegistrySchemaRepository
    {
        /// <summary>
        /// GetApplicationServiceRegistrySchemaListRepository
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetApplicationServiceRegistrySchemaListRepository(PaginationRequest paginationRequest);

        /// <summary>
        /// CheckApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        Task<bool> CheckApplicationServiceRegistrySchemaRepository(ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto);

        /// <summary>
        /// AddApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        Task<ResponseModel<long>> AddApplicationServiceRegistrySchemaRepository(ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto);

        /// <summary>
        /// GetApplicationServiceRegistrySchemaDetailRepository
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        Task<ResponseModel<ApplicationServiceRegistrySchemaDto>> GetApplicationServiceRegistrySchemaDetailRepository(long schemasid);

        /// <summary>
        /// DeleteApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        Task<ResponseModel<bool>> DeleteApplicationServiceRegistrySchemaRepository(long schemasid, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// InActiveApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        Task<ResponseModel<bool>> InActiveApplicationServiceRegistrySchemaRepository(long schemasid, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// GetSchemasByApplicationServiceRepository
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetSchemasByApplicationServiceRepository(long applicationid, long serviceid);
    }
}