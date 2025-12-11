using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Data;
using NETCleanArchApplication.Dtos;
using NETCleanArchApplication.IRepositories;
using NETCleanArchApplication.IServices;
using NETCleanArchApplication.Dtos.Common;

namespace NETCleanArchApplication.Services
{
    public class ApplicationServiceRegistrySchemaService : IApplicationServiceRegistrySchemaService
    {
        private readonly IApplicationServiceRegistrySchemaRepository _applicationServiceRegistrySchemaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationServiceRegistrySchemaService> _logger;

        #region Constructor
        public ApplicationServiceRegistrySchemaService(
            IApplicationServiceRegistrySchemaRepository applicationServiceRegistrySchemaRepository,
            IMapper mapper,
            ILogger<ApplicationServiceRegistrySchemaService> logger)
        {
            _applicationServiceRegistrySchemaRepository = applicationServiceRegistrySchemaRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion
        private IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }
        #region Method
        /// <summary>
        /// GetApplicationServiceRegistrySchemaListService
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetApplicationServiceRegistrySchemaListService(PaginationRequest paginationRequest)
        {
            try
            {
                var data = await _applicationServiceRegistrySchemaRepository.GetApplicationServiceRegistrySchemaListRepository(paginationRequest);
                if (data.IsSuccess)
                {
                    var dtoList = _mapper.Map<List<ApplicationServiceRegistrySchemaDto>>(data.Data);
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(true, AppCommon.GetAllMessage("Application Service Registry Schema"), dtoList);
                }
                else
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(data.IsSuccess, data.Message, new List<ApplicationServiceRegistrySchemaDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(false, AppCommon.ErrorMessage(), new List<ApplicationServiceRegistrySchemaDto>());
            }
        }

        /// <summary>
        /// AddApplicationServiceRegistrySchemaService
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        public async Task<ResponseModel<long>> AddApplicationServiceRegistrySchemaService(ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto)
        {
            try
            {
                bool isvalid = await _applicationServiceRegistrySchemaRepository.CheckApplicationServiceRegistrySchemaRepository(applicationServiceRegistrySchemaDto);
                if (!isvalid)
                {
                    var response = await _applicationServiceRegistrySchemaRepository.AddApplicationServiceRegistrySchemaRepository(applicationServiceRegistrySchemaDto);
                    return response;
                }
                else
                    return new ResponseModel<long>(false, AppCommon.AlreadyExistsMessage("Application Service Registry Schema"), 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<long>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        /// <summary>
        /// GetApplicationServiceRegistrySchemaDetailService         
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<ApplicationServiceRegistrySchemaDto>> GetApplicationServiceRegistrySchemaDetailService(long schemasid)
        {
            try
            {
                var data = await _applicationServiceRegistrySchemaRepository.GetApplicationServiceRegistrySchemaDetailRepository(schemasid);
                if (data.IsSuccess)
                {
                    var dto = _mapper.Map<ApplicationServiceRegistrySchemaDto>(data.Data);
                    return new ResponseModel<ApplicationServiceRegistrySchemaDto>(true, AppCommon.GetMessage("Application Service Registry Schema"), dto);
                }
                else
                    return new ResponseModel<ApplicationServiceRegistrySchemaDto>(data.IsSuccess, data.Message, new ApplicationServiceRegistrySchemaDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return new ResponseModel<ApplicationServiceRegistrySchemaDto>(false, AppCommon.ErrorMessage(), new ApplicationServiceRegistrySchemaDto());
            }
        }

        /// <summary>
        /// DeleteApplicationServiceRegistrySchemaService
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<bool>> DeleteApplicationServiceRegistrySchemaService(long schemasid)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var deleteSchema = await _applicationServiceRegistrySchemaRepository.DeleteApplicationServiceRegistrySchemaRepository(schemasid, connection, transaction);
                if (!deleteSchema.IsSuccess)
                {
                    transaction.Rollback();
                    return deleteSchema;
                }

                transaction.Commit();
                return new ResponseModel<bool>(true, AppCommon.DeleteMessage("Application Service Registry Schema"), true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

       

        /// <summary>
        /// InActiveApplicationServiceRegistrySchemaService
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<bool>> InActiveApplicationServiceRegistrySchemaService(long schemasid)
        {
            using var connection = CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var inactiveSchema = await _applicationServiceRegistrySchemaRepository.InActiveApplicationServiceRegistrySchemaRepository(schemasid, connection, transaction);
                if (!inactiveSchema.IsSuccess)
                {
                    transaction.Rollback();
                    return inactiveSchema;
                }

                transaction.Commit();
                return new ResponseModel<bool>(true, AppCommon.DeactivatMessage("Application Service Registry Schema"), true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        /// <summary>
        /// GetSchemasByApplicationServiceService
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        public async Task<ResponseModel<List<ApplicationServiceRegistrySchemaDto>>> GetSchemasByApplicationServiceService(long applicationid, long serviceid)
        {
            try
            {
                var data = await _applicationServiceRegistrySchemaRepository.GetSchemasByApplicationServiceRepository(applicationid, serviceid);
                if (data.IsSuccess)
                {
                    var dtoList = _mapper.Map<List<ApplicationServiceRegistrySchemaDto>>(data.Data);
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(true, AppCommon.GetAllMessage("Application Service Registry Schema"), dtoList);
                }
                else
                    return new ResponseModel<List<ApplicationServiceRegistrySchemaDto>>(data.IsSuccess, data.Message, new List<ApplicationServiceRegistrySchemaDto>());
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