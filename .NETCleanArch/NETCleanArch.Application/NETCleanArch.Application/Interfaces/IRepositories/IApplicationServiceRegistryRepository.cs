using System.Data;
using NETCleanArchApplication.Dtos;
using NETCleanArchApplication.Dtos.Common;

namespace NETCleanArchApplication.IRepositories
{
    public interface IApplicationServiceRegistryRepository
    {
        /// <summary>
        /// GetApplicationServiceRegistryListRepository
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<ApplicationServiceRegistryDto>>> GetApplicationServiceRegistryListRepository(PaginationRequest paginationRequest);

        /// <summary>
        /// CheckApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="applicationServiceRegistryDto"></param>
        /// <returns></returns>
        Task<bool> CheckApplicationServiceRegistryRepository(ApplicationServiceRegistryDto applicationServiceRegistryDto);

        /// <summary>
        /// AddApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="applicationServiceRegistryDto"></param>
        /// <returns></returns>
        Task<ResponseModel<long>> AddApplicationServiceRegistryRepository(ApplicationServiceRegistryDto applicationServiceRegistryDto);

        /// <summary>
        /// GetApplicationServiceRegistryDetailRepository
        /// </summary>
        /// <param name="serviceregistryid"></param>
        /// <returns></returns>
        Task<ResponseModel<ApplicationServiceRegistryDto>> GetApplicationServiceRegistryDetailRepository(long serviceregistryid);

        /// <summary>
        /// DeleteApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="serviceregistryid"></param>
        /// <returns></returns>
        Task<ResponseModel<bool>> DeleteApplicationServiceRegistryRepository(long serviceregistryid, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// InActiveApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="serviceregistryid"></param>
        /// <returns></returns>
        Task<ResponseModel<bool>> InActiveApplicationServiceRegistryRepository(long serviceregistryid, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// ApplicationServiceRegistryDropDownList
        /// </summary>
        /// <returns></returns>
        //Task<ResponseModel<List<DropDownModel>>> ApplicationServiceRegistryDropDownList();
    }
}