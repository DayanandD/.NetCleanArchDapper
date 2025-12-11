using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;
using NETCleanArch.Application.Interfaces.IRepositories;
using NETCleanArchInfrastructure.CommonQueries;
using NETCleanArchInfrastructure.Data;

namespace NETCleanArchInfrastructure.Repositories
{
    public class ApplicationServiceRegistryRepository : IApplicationServiceRegistryRepository
    {
        private readonly DapperDbContext _dbContext;
        private readonly ILogger<ApplicationServiceRegistryRepository> _logger;
        private readonly AppCommon _appCommon;
        private readonly CommonQuery _commonQuery;

        #region Constructor
        public ApplicationServiceRegistryRepository(
            DapperDbContext dbContext,
            ILogger<ApplicationServiceRegistryRepository> logger,
            AppCommon appCommon,
            CommonQuery commonQuery)
        {
            _dbContext = dbContext;
            _logger = logger;
            _appCommon = appCommon;
            _commonQuery = commonQuery;
        }
        #endregion

        #region Method
        /// <summary>
        /// GetApplicationServiceRegistryListRepository
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<ApplicationServiceRegistryDto>>> GetApplicationServiceRegistryListRepository(PaginationRequest paginationRequest)
        {
            try
            {
                var query = CommonQuery.ApplicationServiceRegistryGetF(0, paginationRequest.PageSize, (paginationRequest.PageNumber - 1) * paginationRequest.PageSize);
                using var connection = _dbContext.CreateConnection();
                var data = await connection.QueryAsync<ApplicationServiceRegistryDto>(query);

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ApplicationServiceRegistryDto>>(true, AppCommon.GetAllMessage("Application Service Registry"), data.ToList());
                }
                else
                {
                    return new ResponseModel<List<ApplicationServiceRegistryDto>>(false, AppCommon.NoData(), new List<ApplicationServiceRegistryDto>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<List<ApplicationServiceRegistryDto>>(false, AppCommon.ErrorMessage(), new List<ApplicationServiceRegistryDto>());
            }
        }

        /// <summary>
        /// CheckApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="applicationServiceRegistryDto"></param>
        /// <returns></returns>
        /// <summary>
        /// CheckApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="applicationServiceRegistryDto"></param>
        /// <returns></returns>
        public async Task<bool> CheckApplicationServiceRegistryRepository(ApplicationServiceRegistryDto applicationServiceRegistryDto)
        {
            try
            {
                var query = CommonQuery.CheckApplicationServiceRegistry(
                    applicationServiceRegistryDto.serviceregistryid,
                    applicationServiceRegistryDto.serviceid,
                    applicationServiceRegistryDto.baseurl
                );

                using var connection = _dbContext.CreateConnection();
                bool isvalid = await connection.ExecuteScalarAsync<bool>(query);
                return isvalid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckApplicationServiceRegistryRepository");
                return false;
            }
        }

        /// <summary>
        /// AddApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="applicationServiceRegistryDto"></param>
        /// <returns></returns>
        public async Task<ResponseModel<long>> AddApplicationServiceRegistryRepository(ApplicationServiceRegistryDto applicationServiceRegistryDto)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"CALL Vms360.applicationserviceregistryp(
                    @ServiceRegistryId, @ServiceId, @ServiceName, @BaseUrl, 
                    @AddEndpoint, @UpdateEndpoint, @DeleteEndpoint, @AddonEndpoint,
                    @IsActive, @RequiresAuth, @TimeoutSeconds, @RetryCount,
                    @OperationContext, @PayloadTemplate, NULL);";

                var parameters = new DynamicParameters();
                parameters.Add("@ServiceRegistryId", applicationServiceRegistryDto.serviceregistryid, DbType.Int64);
                parameters.Add("@ServiceId", applicationServiceRegistryDto.serviceid, DbType.Int64);
                parameters.Add("@ServiceName", applicationServiceRegistryDto.servicename, DbType.String);
                parameters.Add("@BaseUrl", applicationServiceRegistryDto.baseurl, DbType.String);
                parameters.Add("@AddEndpoint", applicationServiceRegistryDto.addendpoint, DbType.String);
                parameters.Add("@UpdateEndpoint", applicationServiceRegistryDto.updateendpoint, DbType.String);
                parameters.Add("@DeleteEndpoint", applicationServiceRegistryDto.deleteendpoint, DbType.String);
                parameters.Add("@AddonEndpoint", applicationServiceRegistryDto.addonendpoint, DbType.String);
                parameters.Add("@IsActive", applicationServiceRegistryDto.isactive, DbType.Boolean);
                parameters.Add("@RequiresAuth", applicationServiceRegistryDto.requiresauth, DbType.Boolean);
                parameters.Add("@TimeoutSeconds", applicationServiceRegistryDto.timeoutseconds, DbType.Int32);
                parameters.Add("@RetryCount", applicationServiceRegistryDto.retrycount, DbType.Int32);
                parameters.Add("@OperationContext", applicationServiceRegistryDto.operationcontext, DbType.String);
                parameters.Add("@PayloadTemplate", applicationServiceRegistryDto.payloadtemplate, DbType.String);

                long id = await connection.ExecuteScalarAsync<long>(query, parameters);
                if (id > 0)
                    return new ResponseModel<long>(true, AppCommon.AddDetailsMessage("Application Service Registry"), id);
                else
                    return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        /// <summary>
        /// GetApplicationServiceRegistryDetailRepository
        /// </summary>
        /// <param name="serviceregistryid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<ApplicationServiceRegistryDto>> GetApplicationServiceRegistryDetailRepository(long serviceregistryid)
        {
            try
            {
                var query = CommonQuery.ApplicationServiceRegistryGetF(serviceregistryid, 0, 0);
                using var connection = _dbContext.CreateConnection();
                var data = await connection.QueryFirstOrDefaultAsync<ApplicationServiceRegistryDto>(query);
                if (data != null)
                {
                    return new ResponseModel<ApplicationServiceRegistryDto>(true, AppCommon.GetMessage("Application Service Registry"), data);
                }
                return new ResponseModel<ApplicationServiceRegistryDto>(false, AppCommon.NoData(), new ApplicationServiceRegistryDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<ApplicationServiceRegistryDto>(false, AppCommon.ErrorMessage(), new ApplicationServiceRegistryDto());
            }
        }

        /// <summary>
        /// DeleteApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="serviceregistryid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<bool>> DeleteApplicationServiceRegistryRepository(long serviceregistryid, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                var query = CommonQuery.DeleteApplicationServiceRegistry(serviceregistryid);
                var result = await connection.ExecuteAsync(query, transaction);
                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeleteMessage("Application Service Registry"), true);
                }
                else
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Application Service Registry"), false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        /// <summary>
        /// InActiveApplicationServiceRegistryRepository
        /// </summary>
        /// <param name="serviceregistryid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<bool>> InActiveApplicationServiceRegistryRepository(long serviceregistryid, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                var query = _commonQuery.InActiveApplicationServiceRegistry(serviceregistryid);

                var result = await connection.ExecuteAsync(query, transaction);
                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Application Service Registry"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Application Service Registry"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        ///// <summary>
        ///// ApplicationServiceRegistryDropDownList
        ///// </summary>
        ///// <returns></returns>
        //public async Task<ResponseModel<List<DropDownModel>>> ApplicationServiceRegistryDropDownList()
        //{
        //    try
        //    {
        //        var query = CommonQuery.ApplicationServiceRegistryDropDownList();
        //        using var connection = _dbContext.CreateConnection();
        //        var data = await connection.QueryAsync<DropDownModel>(query);

        //        if (data != null && data.Any())
        //        {
        //            return new ResponseModel<List<DropDownModel>>(true, AppCommon.GetAllMessage("Application Service Registry"), data.ToList());
        //        }
        //        else
        //        {
        //            return new ResponseModel<List<DropDownModel>>(false, AppCommon.NoData(), new List<DropDownModel>());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "");
        //        return new ResponseModel<List<DropDownModel>>(false, AppCommon.ErrorMessage(), new List<DropDownModel>());
        //    }
        //}
        #endregion
    }
}