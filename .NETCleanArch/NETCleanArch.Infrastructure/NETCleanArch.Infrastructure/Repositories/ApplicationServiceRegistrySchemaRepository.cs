using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Interfaces.IRepositories;
using NETCleanArch.Application.Dtos.Common;
using NETCleanArchInfrastructure.Data;
using NETCleanArchInfrastructure.CommonQueries;

namespace NETCleanArchInfrastructure.Repositories
{
    public class ApplicationServiceRegistrySchemaRepository : IApplicationServiceRegistrySchemaRepository
    {
        private readonly DapperDbContext _dbContext;
        private readonly ILogger<ApplicationServiceRegistrySchemaRepository> _logger;
        private readonly AppCommon _appCommon;
        private readonly CommonQuery _commonQuery;

        #region Constructor
        public ApplicationServiceRegistrySchemaRepository(
            DapperDbContext dbContext,
            ILogger<ApplicationServiceRegistrySchemaRepository> logger,
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
        /// GetApplicationServiceRegistrySchemaListRepository
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetApplicationServiceRegistrySchemaListRepository(PaginationRequest paginationRequest)
        {
            try
            {
                var query = CommonQuery.ApplicationServiceRegistrySchemaGetF(0, 0, 0, paginationRequest.PageSize, (paginationRequest.PageNumber - 1) * paginationRequest.PageSize);
                using var connection = _dbContext.CreateConnection();
                var data = await connection.QueryAsync<ApplicationServiceRegistrySchemaDto>(query);

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(true, AppCommon.GetAllMessage("Application Service Registry Schema"), data.ToList());
                }
                else
                {
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(false, AppCommon.NoData(), new List<ApplicationServiceRegistrySchemaDto>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(false, AppCommon.ErrorMessage(), new List<ApplicationServiceRegistrySchemaDto>());
            }
        }

        /// <summary>
        /// CheckApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        public async Task<bool> CheckApplicationServiceRegistrySchemaRepository(ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto)
        {
            try
            {
                var query = CommonQuery.CheckApplicationServiceRegistrySchema(
                    applicationServiceRegistrySchemaDto.schemasid,
                    applicationServiceRegistrySchemaDto.applicationid,
                    applicationServiceRegistrySchemaDto.serviceid,
                    applicationServiceRegistrySchemaDto.endpoint);
                using var connection = _dbContext.CreateConnection();
                bool isvalid = await connection.ExecuteScalarAsync<Boolean>(query);

                return isvalid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return (false);
            }
        }

        /// <summary>
        /// AddApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        public async Task<ResponseModel<long>> AddApplicationServiceRegistrySchemaRepository(ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"CALL Vms360.applicationserviceregistryschemap(
                    @SchemasId, @ApplicationId, @ServiceId, @Endpoint, 
                    @Method, @SchemaJson, @Version, @IsActive,
                    @CreatedBy, @CreatedOn, NULL);";

                var parameters = new DynamicParameters();
                parameters.Add("@SchemasId", applicationServiceRegistrySchemaDto.schemasid, DbType.Int64);
                parameters.Add("@ApplicationId", applicationServiceRegistrySchemaDto.applicationid, DbType.Int64);
                parameters.Add("@ServiceId", applicationServiceRegistrySchemaDto.serviceid, DbType.Int64);
                parameters.Add("@Endpoint", applicationServiceRegistrySchemaDto.endpoint, DbType.String);
                parameters.Add("@Method", applicationServiceRegistrySchemaDto.method, DbType.String);
                parameters.Add("@SchemaJson", applicationServiceRegistrySchemaDto.schemajson, DbType.String);
                parameters.Add("@Version", applicationServiceRegistrySchemaDto.version, DbType.Int32);
                parameters.Add("@IsActive", applicationServiceRegistrySchemaDto.isactive, DbType.Boolean);
                parameters.Add("@CreatedBy", AppCommon.GetEmployeeCode(), DbType.String);
                parameters.Add("@CreatedOn", AppCommon.GetEntryOn(), DbType.DateTime);

                long id = await connection.ExecuteScalarAsync<long>(query, parameters);
                if (id > 0)
                    return new ResponseModel<long>(true, AppCommon.AddDetailsMessage("Application Service Registry Schema"), id);
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
        /// GetApplicationServiceRegistrySchemaDetailRepository
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<ApplicationServiceRegistrySchemaDto>> GetApplicationServiceRegistrySchemaDetailRepository(long schemasid)
        {
            try
            {
                var query = CommonQuery.ApplicationServiceRegistrySchemaGetF(schemasid, 0, 0, 0, 0);
                using var connection = _dbContext.CreateConnection();
                var data = await connection.QueryFirstOrDefaultAsync<ApplicationServiceRegistrySchemaDto>(query);
                if (data != null)
                {
                    return new ResponseModel<ApplicationServiceRegistrySchemaDto>(true, AppCommon.GetMessage("Application Service Registry Schema"), data);
                }
                return new ResponseModel<ApplicationServiceRegistrySchemaDto>(false, AppCommon.NoData(), new ApplicationServiceRegistrySchemaDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<ApplicationServiceRegistrySchemaDto>(false, AppCommon.ErrorMessage(), new ApplicationServiceRegistrySchemaDto());
            }
        }

        /// <summary>
        /// DeleteApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<bool>> DeleteApplicationServiceRegistrySchemaRepository(long schemasid, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                var query = CommonQuery.DeleteApplicationServiceRegistrySchema(schemasid);
                var result = await connection.ExecuteAsync(query, transaction);
                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeleteMessage("Application Service Registry Schema"), true);
                }
                else
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Application Service Registry Schema"), false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        /// <summary>
        /// InActiveApplicationServiceRegistrySchemaRepository
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<bool>> InActiveApplicationServiceRegistrySchemaRepository(long schemasid, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                var query = _commonQuery.InActiveApplicationServiceRegistrySchema(schemasid);

                var result = await connection.ExecuteAsync(query, transaction);
                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Application Service Registry Schema"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Application Service Registry Schema"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        /// <summary>
        /// GetSchemasByApplicationServiceRepository
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetSchemasByApplicationServiceRepository(long applicationid, long serviceid)
        {
            try
            {
                var query = CommonQuery.ApplicationServiceRegistrySchemaGetF(0, applicationid, serviceid, 0, 0);
                using var connection = _dbContext.CreateConnection();
                var data = await connection.QueryAsync<ApplicationServiceRegistrySchemaDto>(query);

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(true, AppCommon.GetAllMessage("Application Service Registry Schema"), data.ToList());
                }
                else
                {
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(false, AppCommon.NoData(), new List<ApplicationServiceRegistrySchemaDto>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(false, AppCommon.ErrorMessage(), new List<ApplicationServiceRegistrySchemaDto>());
            }
        }
        #endregion
    }
}