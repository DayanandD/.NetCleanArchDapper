using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NETCleanArchApplication.Dtos.Common;
using NETCleanArchApplication.Interfaces.IRepositories;
using NETCleanArchDomain.Entities;
using NETCleanArchInfrastructure.Data;

namespace NETCleanArchInfrastructure.Repositories
{
    public class ServiceRegistryRepository : IServiceRegistryRepository
    {
        #region Constructor
        private readonly DapperDbContext _dbContext;
        private readonly ILogger<ServiceRegistryRepository> _logger;
        private readonly AppCommon _appCommon;

        public ServiceRegistryRepository(
            DapperDbContext dbContext,
            ILogger<ServiceRegistryRepository> logger,
            AppCommon appCommon)
        {
            _dbContext = dbContext;
            _logger = logger;
            _appCommon = appCommon;
        }
        #endregion

        #region Service Registry Methods

        public async Task<ResponseModel<List<ServiceRegistry>>> GetAllServiceRegistry(PaginationRequest paginationRequest)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        sr.serviceregistryid,
                        sr.serviceid,
                        sr.servicename,
                        sr.baseurl,
                        sr.addendpoint,
                        sr.updateendpoint,
                        sr.deleteendpoint,
                        sr.addonendpoint,
                        sr.isactive,
                        sr.requiresauth,
                        sr.timeoutseconds,
                        sr.retrycount,
                        sr.createdat,
                        sr.updatedat,
                        sr.operationcontext::text as operationcontext,
                        sr.payloadtemplate::text as payloadtemplate
                    FROM NETCleanArchserviceregistry sr
                    WHERE sr.isactive = true
                    ORDER BY sr.createdat DESC
                    LIMIT @PageSize OFFSET @Offset";

                var parameters = new
                {
                    PageSize = paginationRequest.PageSize,
                    Offset = (paginationRequest.PageNumber - 1) * paginationRequest.PageSize
                };

                var data = (await connection.QueryAsync<ServiceRegistry>(query, parameters)).ToList();

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ServiceRegistry>>(true, AppCommon.GetAllMessage("Service Registry"), data);
                }
                else
                {
                    return new ResponseModel<List<ServiceRegistry>>(false, AppCommon.NoData(), new List<ServiceRegistry>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllServiceRegistry");
                return new ResponseModel<List<ServiceRegistry>>(false, AppCommon.ErrorMessage(), new List<ServiceRegistry>());
            }
        }

        public async Task<ResponseModel<long>> AddServiceRegistry(ServiceRegistry serviceRegistry)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    INSERT INTO NETCleanArchserviceregistry (
                        serviceid, servicename, baseurl, addendpoint, updateendpoint, 
                        deleteendpoint, addonendpoint, isactive, requiresauth, 
                        timeoutseconds, retrycount, operationcontext, payloadtemplate, 
                        createdat, updatedat
                    )
                    VALUES (
                        @ServiceId, @ServiceName, @BaseUrl, @AddEndpoint, @UpdateEndpoint,
                        @DeleteEndpoint, @AddonEndpoint, @IsActive, @RequiresAuth,
                        @TimeoutSeconds, @RetryCount, @OperationContext::jsonb, @PayloadTemplate::jsonb,
                        @CreatedAt, @UpdatedAt
                    )
                    RETURNING serviceregistryid";

                var parameters = new
                {
                    ServiceId = serviceRegistry.ServiceId,
                    ServiceName = serviceRegistry.ServiceName,
                    BaseUrl = serviceRegistry.BaseUrl,
                    AddEndpoint = serviceRegistry.AddEndpoint,
                    UpdateEndpoint = serviceRegistry.UpdateEndpoint,
                    DeleteEndpoint = serviceRegistry.DeleteEndpoint,
                    AddonEndpoint = serviceRegistry.AddonEndpoint,
                    IsActive = true,
                    RequiresAuth = serviceRegistry.RequiresAuth,
                    TimeoutSeconds = serviceRegistry.TimeoutSeconds,
                    RetryCount = serviceRegistry.RetryCount,
                    OperationContext = serviceRegistry.OperationContext ?? "{}",
                    PayloadTemplate = serviceRegistry.PayloadTemplate ?? "{}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                long id = await connection.ExecuteScalarAsync<long>(query, parameters);

                if (id > 0)
                {
                    return new ResponseModel<long>(true, AppCommon.AddDetailsMessage("Service Registry"), id);
                }
                else
                {
                    return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddServiceRegistry");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        public async Task<ResponseModel<ServiceRegistry>> GetServiceRegistryById(long serviceRegistryId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        serviceregistryid, serviceid, servicename, baseurl,
                        addendpoint, updateendpoint, deleteendpoint, addonendpoint,
                        isactive, requiresauth, timeoutseconds, retrycount,
                        createdat, updatedat,
                        operationcontext::text as operationcontext,
                        payloadtemplate::text as payloadtemplate
                    FROM NETCleanArchserviceregistry
                    WHERE serviceregistryid = @ServiceRegistryId";

                var data = await connection.QueryFirstOrDefaultAsync<ServiceRegistry>(query,
                    new { ServiceRegistryId = serviceRegistryId });

                return new ResponseModel<ServiceRegistry>(true, AppCommon.GetMessage("Service Registry"), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceRegistryById");
                return new ResponseModel<ServiceRegistry>(false, AppCommon.ErrorMessage(), new ServiceRegistry());
            }
        }

        public async Task<ResponseModel<bool>> UpdateServiceRegistry(ServiceRegistry serviceRegistry)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    UPDATE NETCleanArchserviceregistry 
                    SET serviceid = @ServiceId,
                        servicename = @ServiceName,
                        baseurl = @BaseUrl,
                        addendpoint = @AddEndpoint,
                        updateendpoint = @UpdateEndpoint,
                        deleteendpoint = @DeleteEndpoint,
                        addonendpoint = @AddonEndpoint,
                        requiresauth = @RequiresAuth,
                        timeoutseconds = @TimeoutSeconds,
                        retrycount = @RetryCount,
                        operationcontext = @OperationContext::jsonb,
                        payloadtemplate = @PayloadTemplate::jsonb,
                        updatedat = @UpdatedAt
                    WHERE serviceregistryid = @ServiceRegistryId";

                var parameters = new
                {
                    ServiceRegistryId = serviceRegistry.ServiceRegistryID,
                    ServiceId = serviceRegistry.ServiceId,
                    ServiceName = serviceRegistry.ServiceName,
                    BaseUrl = serviceRegistry.BaseUrl,
                    AddEndpoint = serviceRegistry.AddEndpoint,
                    UpdateEndpoint = serviceRegistry.UpdateEndpoint,
                    DeleteEndpoint = serviceRegistry.DeleteEndpoint,
                    AddonEndpoint = serviceRegistry.AddonEndpoint,
                    RequiresAuth = serviceRegistry.RequiresAuth,
                    TimeoutSeconds = serviceRegistry.TimeoutSeconds,
                    RetryCount = serviceRegistry.RetryCount,
                    OperationContext = serviceRegistry.OperationContext ?? "{}",
                    PayloadTemplate = serviceRegistry.PayloadTemplate ?? "{}",
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await connection.ExecuteAsync(query, parameters);

                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.UpdateDetailsMessage("Service Registry"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Service Registry"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateServiceRegistry");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<ResponseModel<bool>> DeleteServiceRegistry(long serviceRegistryId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    UPDATE NETCleanArchserviceregistry 
                    SET isactive = false, updatedat = @UpdatedAt
                    WHERE serviceregistryid = @ServiceRegistryId";

                var result = await connection.ExecuteAsync(query, new
                {
                    ServiceRegistryId = serviceRegistryId,
                    UpdatedAt = DateTime.UtcNow
                });

                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Service Registry"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Service Registry"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceRegistry");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<bool> CheckServiceRegistryExists(long serviceRegistryId, long? serviceId, string baseUrl)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT EXISTS(
                        SELECT 1 FROM NETCleanArchserviceregistry 
                        WHERE serviceid = @ServiceId 
                        AND LOWER(baseurl) = LOWER(@BaseUrl)
                        AND serviceregistryid != @ServiceRegistryId
                        AND isactive = true
                    )";

                bool exists = await connection.ExecuteScalarAsync<bool>(query, new
                {
                    ServiceRegistryId = serviceRegistryId,
                    ServiceId = serviceId,
                    BaseUrl = baseUrl
                });

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking service registry existence");
                return false;
            }
        }

        #endregion

        #region Service Schema Methods

        public async Task<ResponseModel<List<ServiceSchema>>> GetAllServiceSchemas(PaginationRequest paginationRequest)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        ss.schemasid as schemaid,
                        ss.applicationid,
                        ss.serviceid,
                        ss.endpoint,
                        ss.method,
                        ss.schemajson::text as schemajson,
                        ss.version,
                        ss.isactive,
                        ss.createdby,
                        ss.createdon,
                        ss.updatedby,
                        ss.updatedon,
                        a.applicationname,
                        s.servicename
                    FROM NETCleanArchservice_schema ss
                    LEFT JOIN NETCleanArchapplicationmaster a ON ss.applicationid = a.applicationid
                    LEFT JOIN NETCleanArchservicemaster s ON ss.serviceid = s.serviceid
                    WHERE ss.isactive = true
                    ORDER BY ss.createdon DESC
                    LIMIT @PageSize OFFSET @Offset";

                var parameters = new
                {
                    PageSize = paginationRequest.PageSize,
                    Offset = (paginationRequest.PageNumber - 1) * paginationRequest.PageSize
                };

                var data = (await connection.QueryAsync<ServiceSchema>(query, parameters)).ToList();

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ServiceSchema>>(true, AppCommon.GetAllMessage("Service Schema"), data);
                }
                else
                {
                    return new ResponseModel<List<ServiceSchema>>(false, AppCommon.NoData(), new List<ServiceSchema>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllServiceSchemas");
                return new ResponseModel<List<ServiceSchema>>(false, AppCommon.ErrorMessage(), new List<ServiceSchema>());
            }
        }

        public async Task<ResponseModel<List<ServiceSchema>>> GetSchemasByApplicationId(long applicationId, PaginationRequest paginationRequest)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        ss.schemasid as schemaid,
                        ss.applicationid,
                        ss.serviceid,
                        ss.endpoint,
                        ss.method,
                        ss.schemajson::text as schemajson,
                        ss.version,
                        ss.isactive,
                        ss.createdby,
                        ss.createdon,
                        a.applicationname,
                        s.servicename
                    FROM NETCleanArchservice_schema ss
                    LEFT JOIN NETCleanArchapplicationmaster a ON ss.applicationid = a.applicationid
                    LEFT JOIN NETCleanArchservicemaster s ON ss.serviceid = s.serviceid
                    WHERE ss.applicationid = @ApplicationId AND ss.isactive = true
                    ORDER BY ss.createdon DESC
                    LIMIT @PageSize OFFSET @Offset";

                var parameters = new
                {
                    ApplicationId = applicationId,
                    PageSize = paginationRequest.PageSize,
                    Offset = (paginationRequest.PageNumber - 1) * paginationRequest.PageSize
                };

                var data = (await connection.QueryAsync<ServiceSchema>(query, parameters)).ToList();

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ServiceSchema>>(true, AppCommon.GetAllMessage("Service Schema"), data);
                }
                else
                {
                    return new ResponseModel<List<ServiceSchema>>(false, AppCommon.NoData(), new List<ServiceSchema>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSchemasByApplicationId");
                return new ResponseModel<List<ServiceSchema>>(false, AppCommon.ErrorMessage(), new List<ServiceSchema>());
            }
        }

        public async Task<ResponseModel<List<ServiceSchema>>> GetSchemasByServiceId(long serviceId, PaginationRequest paginationRequest)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        ss.schemasid as schemaid,
                        ss.applicationid,
                        ss.serviceid,
                        ss.endpoint,
                        ss.method,
                        ss.schemajson::text as schemajson,
                        ss.version,
                        ss.isactive,
                        ss.createdby,
                        ss.createdon,
                        a.applicationname,
                        s.servicename
                    FROM NETCleanArchservice_schema ss
                    LEFT JOIN NETCleanArchapplicationmaster a ON ss.applicationid = a.applicationid
                    LEFT JOIN NETCleanArchservicemaster s ON ss.serviceid = s.serviceid
                    WHERE ss.serviceid = @ServiceId AND ss.isactive = true
                    ORDER BY ss.createdon DESC
                    LIMIT @PageSize OFFSET @Offset";

                var parameters = new
                {
                    ServiceId = serviceId,
                    PageSize = paginationRequest.PageSize,
                    Offset = (paginationRequest.PageNumber - 1) * paginationRequest.PageSize
                };

                var data = (await connection.QueryAsync<ServiceSchema>(query, parameters)).ToList();

                if (data != null && data.Any())
                {
                    return new ResponseModel<List<ServiceSchema>>(true, AppCommon.GetAllMessage("Service Schema"), data);
                }
                else
                {
                    return new ResponseModel<List<ServiceSchema>>(false, AppCommon.NoData(), new List<ServiceSchema>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSchemasByServiceId");
                return new ResponseModel<List<ServiceSchema>>(false, AppCommon.ErrorMessage(), new List<ServiceSchema>());
            }
        }

        public async Task<ResponseModel<long>> AddServiceSchema(ServiceSchema serviceSchema)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    INSERT INTO NETCleanArchservice_schema (
                        applicationid, serviceid, endpoint, method, schemajson,
                        version, isactive, createdby, createdon
                    )
                    VALUES (
                        @ApplicationId, @ServiceId, @Endpoint, @Method, @SchemaJson::jsonb,
                        @Version, @IsActive, @CreatedBy, @CreatedOn
                    )
                    RETURNING schemasid";

                var parameters = new
                {
                    ApplicationId = serviceSchema.ApplicationId,
                    ServiceId = serviceSchema.ServiceId,
                    Endpoint = serviceSchema.Endpoint,
                    Method = serviceSchema.Method,
                    SchemaJson = serviceSchema.SchemaJson ?? "{}",
                    Version = 1,
                    IsActive = true,
                    //CreatedBy = _appCommon.GetEmployeeCode(),
                    CreatedOn = AppCommon.GetEntryOn()
                };

                long id = await connection.ExecuteScalarAsync<long>(query, parameters);

                if (id > 0)
                {
                    return new ResponseModel<long>(true, AppCommon.AddDetailsMessage("Service Schema"), id);
                }
                else
                {
                    return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddServiceSchema");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        public async Task<ResponseModel<ServiceSchema>> GetServiceSchemaById(long schemaId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT 
                        ss.schemasid as schemaid,
                        ss.applicationid,
                        ss.serviceid,
                        ss.endpoint,
                        ss.method,
                        ss.schemajson::text as schemajson,
                        ss.version,
                        ss.isactive,
                        ss.createdby,
                        ss.createdon,
                        a.applicationname,
                        s.servicename
                    FROM NETCleanArchservice_schema ss
                    LEFT JOIN NETCleanArchapplicationmaster a ON ss.applicationid = a.applicationid
                    LEFT JOIN NETCleanArchservicemaster s ON ss.serviceid = s.serviceid
                    WHERE ss.schemasid = @SchemaId";

                var data = await connection.QueryFirstOrDefaultAsync<ServiceSchema>(query,
                    new { SchemaId = schemaId });

                return new ResponseModel<ServiceSchema>(true, AppCommon.GetMessage("Service Schema"), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceSchemaById");
                return new ResponseModel<ServiceSchema>(false, AppCommon.ErrorMessage(), new ServiceSchema());
            }
        }

        public async Task<ResponseModel<bool>> UpdateServiceSchema(ServiceSchema serviceSchema)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    UPDATE NETCleanArchservice_schema 
                    SET applicationid = @ApplicationId,
                        serviceid = @ServiceId,
                        endpoint = @Endpoint,
                        method = @Method,
                        schemajson = @SchemaJson::jsonb,
                        version = version + 1,
                        updatedby = @UpdatedBy,
                        updatedon = @UpdatedOn
                    WHERE schemasid = @SchemaId";

                var parameters = new
                {
                    SchemaId = serviceSchema.SchemaId,
                    ApplicationId = serviceSchema.ApplicationId,
                    ServiceId = serviceSchema.ServiceId,
                    Endpoint = serviceSchema.Endpoint,
                    Method = serviceSchema.Method,
                    SchemaJson = serviceSchema.SchemaJson ?? "{}",
                    //UpdatedBy = _appCommon.GetEmployeeCode(),
                    UpdatedOn = AppCommon.GetEntryOn()
                };

                var result = await connection.ExecuteAsync(query, parameters);

                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.UpdateDetailsMessage("Service Schema"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Service Schema"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateServiceSchema");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<ResponseModel<bool>> DeleteServiceSchema(long schemaId)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    UPDATE NETCleanArchservice_schema 
                    SET isactive = false,
                        updatedby = @UpdatedBy,
                        updatedon = @UpdatedOn
                    WHERE schemasid = @SchemaId";

                var result = await connection.ExecuteAsync(query, new
                {
                    SchemaId = schemaId,
                    //UpdatedBy = _appCommon.GetEmployeeCode(),
                    UpdatedOn = AppCommon.GetEntryOn()
                });

                if (result > 0)
                {
                    return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Service Schema"), true);
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.NotFoundMessage("Service Schema"), false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceSchema");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        public async Task<bool> CheckSchemaExists(long schemaId, long applicationId, long serviceId, string endpoint)
        {
            try
            {
                using var connection = _dbContext.CreateConnection();

                var query = @"
                    SELECT EXISTS(
                        SELECT 1 FROM NETCleanArchservice_schema 
                        WHERE applicationid = @ApplicationId 
                        AND serviceid = @ServiceId
                        AND LOWER(endpoint) = LOWER(@Endpoint)
                        AND schemasid != @SchemaId
                        AND isactive = true
                    )";

                bool exists = await connection.ExecuteScalarAsync<bool>(query, new
                {
                    SchemaId = schemaId,
                    ApplicationId = applicationId,
                    ServiceId = serviceId,
                    Endpoint = endpoint
                });

                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking schema existence");
                return false;
            }
        }

        #endregion
    }
}
