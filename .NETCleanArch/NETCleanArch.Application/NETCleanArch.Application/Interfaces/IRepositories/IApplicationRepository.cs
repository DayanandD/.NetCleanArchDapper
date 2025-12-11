using System.Data;
using NETCleanArchApplication.Dtos.Common;
using NETCleanArchDomain.Entities;

namespace NETCleanArchApplication.Interfaces.IRepositories
{
    public interface IApplicationRepository
    {
        // <summary>GetAllApplication</summary>
        Task<ResponseModel<List<ApplicationMaster>>> GetAllApplication(PaginationRequest paginationRequest);

        /// <summary>CheckApplicationName</summary>
        Task<bool> CheckApplicationName(ApplicationMaster applicationMaster);

        /// <summary>AddApplication</summary>
        Task<ResponseModel<long>> AddApplication(ApplicationMaster application);

        /// <summary>AddApplicationServiceMapping</summary>
        Task<ResponseModel<bool>> AddApplicationServiceMapping(long applicationId, int[] serviceIds);

        /// <summary>GetApplicationById</summary>
        Task<ResponseModel<ApplicationMaster>> GetApplicationById(long applicationId);

        /// <summary>InActiveApplicationRepository</summary>
        Task<ResponseModel<bool>> InActiveApplicationRepository(long applicationId);

        /// <summary>TotalActiveApplicationMapping</summary>
        Task<ResponseModel<int>> TotalActiveApplicationMapping(long applicationId);

        /// <summary>LogServiceSyncAsync</summary>
        Task<bool> LogServiceSyncAsync(long applicationId, string serviceName, string operation, bool isSuccess, IDbConnection connection, IDbTransaction transaction);

        /// <summary>QueueBackgroundOperationsAsync</summary>
        Task<bool> QueueBackgroundOperationsAsync(long applicationId, int[] serviceIds, IDbConnection connection, IDbTransaction transaction);

    }
}
