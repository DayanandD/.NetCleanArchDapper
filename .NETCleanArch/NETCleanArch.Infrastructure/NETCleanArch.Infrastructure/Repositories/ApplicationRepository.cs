using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using NETCleanArchApplication.Dtos.Common;
using NETCleanArchApplication.Interfaces.IRepositories;
using NETCleanArchDomain.Entities;
using NETCleanArchInfrastructure.Data;
using static System.Net.Mime.MediaTypeNames;

namespace NETCleanArchInfrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        #region Constructor
        private readonly DapperDbContext _dbContext;
        private readonly ILogger<ApplicationRepository> _logger;
        private readonly AppCommon _appCommon;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationRepository(
            DapperDbContext dbContext,
            ILogger<ApplicationRepository> logger,
            AppCommon appCommon,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _logger = logger;
            _appCommon = appCommon;
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Methods

        /// <summary>GetAllApplication</summary>
        public async Task<ResponseModel<List<ApplicationMaster>>> GetAllApplication(PaginationRequest paginationRequest)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        a.applicationid,
                        a.applicationname,
                        a.description,
                        a.isactive,
                        a.createdby,
                        a.createdon,
                        STRING_AGG(s.servicename, ', ') as services
                    FROM NETCleanArchapplicationmaster a
                    LEFT JOIN NETCleanArchapplicationservicemapping asm ON a.applicationid = asm.applicationid AND asm.isactive = true
                    LEFT JOIN NETCleanArchservicemaster s ON asm.serviceid = s.serviceid
                    WHERE a.isactive = true
                    GROUP BY a.applicationid, a.applicationname, a.description, a.isactive, a.createdby, a.createdon
                    ORDER BY a.createdon DESC
                    LIMIT @PageSize OFFSET @Offset";

                var parameters = new
                {
                    PageSize = paginationRequest.PageSize,
                    Offset = (paginationRequest.PageNumber - 1) * paginationRequest.PageSize
                };

                var data = (await connection.QueryAsync<ApplicationMaster>(query, parameters)).ToList();

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ApplicationMaster>>(true, AppCommon.GetAllMessage("Application"), data);
                }
                else
                {
                    return new ResponseModel<List<ApplicationMaster>>(false, AppCommon.NoData(), new List<ApplicationMaster>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllApplication");
                return new ResponseModel<List<ApplicationMaster>>(false, AppCommon.ErrorMessage(), new List<ApplicationMaster>());
            }
        }

        /// <summary>CheckApplicationName</summary>
        public async Task<bool> CheckApplicationName(ApplicationMaster applicationMaster)
        {
            try
            {
                var query = @"
                    SELECT EXISTS(
                        SELECT 1 FROM NETCleanArchapplicationmaster 
                        WHERE LOWER(applicationname) = LOWER(@ApplicationName) 
                        AND applicationid != @ApplicationId
                        AND isactive = true
                    )";

                using var connection = _dbContext.CreateConnection();
                bool isValid = await connection.ExecuteScalarAsync<bool>(query, new
                {
                    ApplicationName = applicationMaster.ApplicationName,
                    ApplicationId = applicationMaster.ApplicationId
                });
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking application name");
                return false;
            }
        }

        /// <summary>AddApplication</summary>
        /// <summary>AddApplication</summary>
        public async Task<ResponseModel<long>> AddApplication(ApplicationMaster application)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
            INSERT INTO NETCleanArchapplicationmaster (applicationname, description, isactive, createdby, createdon)
            VALUES (@ApplicationName, @Description, @IsActive, @CreatedBy, @CreatedOn)
            RETURNING applicationid";

                
                var parameters = new
                {
                    ApplicationName = application.ApplicationName,
                    Description = application.Description,
                    IsActive = true,
                    CreatedBy =application.CreatedBy, // Uncommented this line
                    CreatedOn = AppCommon.GetEntryOn()
                };

                long applicationId = await connection.ExecuteScalarAsync<long>(query, parameters);

                if (applicationId > 0)
                {
                    return new ResponseModel<long>(true, AppCommon.AddDetailsMessage("Application"), applicationId);
                }
                else
                {
                    return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddApplication");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        /// <summary>AddApplicationServiceMapping</summary>
        /// <summary>AddApplicationServiceMapping</summary>
        public async Task<ResponseModel<bool>> AddApplicationServiceMapping(long applicationId, int[] serviceIds)
        {
            IDbTransaction transaction = null;
            IDbConnection connection = null;

            try
            {
                connection = _dbContext.CreateConnection();
                //await connection.OpenAsync();
                transaction = connection.BeginTransaction();

                string selectQuery = @"
            SELECT serviceid 
            FROM NETCleanArchapplicationservicemapping 
            WHERE applicationid = @ApplicationId";

                var existingServiceIds = (await connection.QueryAsync<int>(selectQuery, new { ApplicationId = applicationId }, transaction)).ToList();

                var newServiceList = serviceIds?.ToList() ?? new List<int>();
                var serviceIdsToDelete = existingServiceIds.Except(newServiceList).ToList();
                var serviceIdsToInsert = newServiceList.Except(existingServiceIds).ToList();

                if (serviceIdsToDelete.Any())
                {
                    string deleteQuery = @"
                DELETE FROM NETCleanArchapplicationservicemapping 
                WHERE applicationid = @ApplicationId AND serviceid = ANY(@ServiceIds)";

                    await connection.ExecuteAsync(deleteQuery, new
                    {
                        ApplicationId = applicationId,
                        ServiceIds = serviceIdsToDelete.ToArray()
                    }, transaction);
                }

                if (serviceIdsToInsert.Any())
                {
                    string insertQuery = @"
                INSERT INTO NETCleanArchapplicationservicemapping (applicationid, serviceid, isactive, createdby, createdon) 
                VALUES (@ApplicationId, @ServiceId, @IsActive, @CreatedBy, @CreatedOn)";

                    foreach (var serviceId in serviceIdsToInsert)
                    {
                        await connection.ExecuteAsync(insertQuery, new
                        {
                            ApplicationId = applicationId,
                            ServiceId = serviceId,
                            IsActive = true,
                            CreatedOn = AppCommon.GetEntryOn()
                        }, transaction);
                    }
                }

                transaction.Commit();
                return new ResponseModel<bool>(true, AppCommon.AddDetailsMessage("Application Service Mapping"), true);
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                _logger.LogError(ex, "Error in AddApplicationServiceMapping for ApplicationId: {ApplicationId}", applicationId);
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
            finally
            {
                transaction?.Dispose();
                connection?.Dispose();
            }
        }

        /// <summary>GetApplicationById</summary>
        public async Task<ResponseModel<ApplicationMaster>> GetApplicationById(long applicationId)
        {
            try
            {
                var query = @"
                    SELECT 
                        a.applicationid,
                        a.applicationname,
                        a.description,
                        a.isactive,
                        a.createdby,
                        a.createdon,
                        STRING_AGG(s.servicename, ', ') as services
                    FROM NETCleanArchapplicationmaster a
                    LEFT JOIN NETCleanArchapplicationservicemapping asm ON a.applicationid = asm.applicationid AND asm.isactive = true
                    LEFT JOIN NETCleanArchservicemaster s ON asm.serviceid = s.serviceid
                    WHERE a.applicationid = @ApplicationId
                    GROUP BY a.applicationid, a.applicationname, a.description, a.isactive, a.createdby, a.createdon";

                using var connection = _dbContext.CreateConnection();
                var data = await connection.QueryFirstOrDefaultAsync<ApplicationMaster>(query, new { ApplicationId = applicationId });

                return new ResponseModel<ApplicationMaster>(true, AppCommon.GetMessage("Application"), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetApplicationById");
                return new ResponseModel<ApplicationMaster>(false, AppCommon.ErrorMessage(), new ApplicationMaster());
            }
        }

        /// <summary>InActiveApplicationRepository</summary>
        public async Task<ResponseModel<bool>> InActiveApplicationRepository(long applicationId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var query = @"
                    UPDATE NETCleanArchapplicationmaster 
                    SET isactive = false, 
                        updatedby = @UpdatedBy, 
                        updatedon = @UpdatedOn
                    WHERE applicationid = @ApplicationId";

                var result = await connection.ExecuteAsync(query, new
                {
                    ApplicationId = applicationId,
                    //UpdatedBy = _appCommon.GetEmployeeCode(),
                    UpdatedOn = AppCommon.GetEntryOn()
                });

                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Application"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Application"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InActiveApplicationRepository");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        /// <summary>TotalActiveApplicationMapping</summary>
        public async Task<ResponseModel<int>> TotalActiveApplicationMapping(long applicationId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();
                var query = @"
                    SELECT COUNT(*) 
                    FROM NETCleanArchapplicationservicemapping 
                    WHERE applicationid = @ApplicationId AND isactive = true";

                int count = await connection.QuerySingleAsync<int>(query, new { ApplicationId = applicationId });

                if (count > 0)
                {
                    return new ResponseModel<int>(true, AppCommon.CountMessage("Application"), count);
                }
                else
                {
                    return new ResponseModel<int>(false, AppCommon.NotFoundMessage("Application"), count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TotalActiveApplicationMapping");
                return new ResponseModel<int>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        #region Logging webhooks

        public async Task<bool> LogServiceSyncAsync(long applicationId, string serviceName, string operation, bool isSuccess, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                var serviceId = await GetServiceIdByNameAsync(serviceName, connection, transaction);

                if (serviceId == 0)
                {
                    _logger.LogWarning("Service ID not found for service name: {ServiceName}", serviceName);
                    return false;
                }

                var logQuery = @"
                    INSERT INTO NETCleanArchservicesynclog 
                    (applicationid, serviceid, operation, issuccess, statuscode, syncedat)
                    VALUES (@ApplicationId, @ServiceId, @Operation, @IsSuccess, @StatusCode, @SyncedAt)";

                var parameters = new
                {
                    ApplicationId = applicationId,
                    ServiceId = serviceId,
                    Operation = operation,
                    IsSuccess = isSuccess,
                    StatusCode = isSuccess ? 200 : 500,
                    SyncedAt = DateTime.UtcNow
                };

                var result = await connection.ExecuteAsync(logQuery, parameters, transaction);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to log sync operation for application {ApplicationId}, service {ServiceName}", applicationId, serviceName);
                return false;
            }
        }

        public async Task<bool> QueueBackgroundOperationsAsync(long applicationId, int[] serviceIds, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                foreach (var serviceId in serviceIds)
                {
                    var logQuery = @"
                        INSERT INTO NETCleanArchservicesynclog 
                        (applicationid, serviceid, operation, issuccess, statuscode, syncedat)
                        VALUES (@ApplicationId, @ServiceId, @Operation, @IsSuccess, @StatusCode, @SyncedAt)";

                    var parameters = new
                    {
                        ApplicationId = applicationId,
                        ServiceId = serviceId,
                        Operation = "BackgroundOperationsQueued",
                        IsSuccess = true,
                        StatusCode = 202,
                        SyncedAt = DateTime.UtcNow
                    };

                    await connection.ExecuteAsync(logQuery, parameters, transaction);
                }

                //_ = Task.Run(async () =>
                //{
                //    try
                //    {
                //        using var scope = _serviceProvider.CreateScope();
                //        var queueService = scope.ServiceProvider.GetRequiredService<IWebhookQueueService>();

                //        await queueService.QueueWebhook("application.background.operations", new
                //        {
                //            ApplicationId = applicationId,
                //            Operations = new[] { "SendWelcomeEmail", "UpdateAnalytics", "InitializeReports" },
                //            Timestamp = DateTime.UtcNow
                //        }, serviceIds.Select(id => (long)id));

                //        _logger.LogInformation("Queued background tasks for application {ApplicationId}", applicationId);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogWarning(ex, "Failed to queue background tasks for application {ApplicationId}", applicationId);
                //    }
                //});

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to queue background operations for application {ApplicationId}", applicationId);
                return false;
            }
        }

        private async Task<long> GetServiceIdByNameAsync(string serviceName, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                var query = @"
                    SELECT serviceid 
                    FROM NETCleanArchserviceregistry 
                    WHERE servicename = @ServiceName 
                    LIMIT 1";

                var parameters = new { ServiceName = serviceName };
                var serviceId = await connection.QueryFirstOrDefaultAsync<long?>(query, parameters, transaction);

                return serviceId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get service ID for service name: {ServiceName}", serviceName);
                return 0;
            }
        }

        #endregion

        #endregion
    }
}
