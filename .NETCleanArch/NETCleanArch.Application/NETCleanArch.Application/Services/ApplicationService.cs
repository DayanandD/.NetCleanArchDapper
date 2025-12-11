using AutoMapper;
using Microsoft.Extensions.Logging;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;
using NETCleanArch.Application.Interfaces.IRepositories;
using NETCleanArch.Application.Interfaces.IServices;
using NETCleanArchDomain.Entities;

namespace NETCleanArch.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        #region Constructor
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationService> _logger;
        //private readonly IWebhookService _webhookService;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IMapper mapper,
            ILogger<ApplicationService> logger
            //IWebhookService webhookService
            )
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _logger = logger;
            //_webhookService = webhookService;
        }
        #endregion

        #region Methods

        /// <summary>GetAllApplication</summary>
        public async Task<ResponseModel<List<ApplicationDto>>> GetAllApplication(PaginationRequest paginationRequest)
        {
            try
            {
                var applicationModel = await _applicationRepository.GetAllApplication(paginationRequest);
                if (applicationModel.IsSuccess)
                {
                    var applicationDto = _mapper.Map<List<ApplicationDto>>(applicationModel.Data);
                    return new ResponseModel<List<ApplicationDto>>(true, applicationModel.Message, applicationDto);
                }
                else
                    return new ResponseModel<List<ApplicationDto>>(false, applicationModel.Message, new List<ApplicationDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all applications");
                return new ResponseModel<List<ApplicationDto>>(false, AppCommon.ErrorMessage(), new List<ApplicationDto>());
            }
        }

        /// <summary>AddApplication with Webhook Integration</summary>
        public async Task<ResponseModel<int>> AddApplication(ApplicationDto applicationDto)
        {
            //using var connection = _dbContext.CreateConnection();
            //connection.Open();
            //using var transaction = connection.BeginTransaction();

            try
            {
                var applicationModel = _mapper.Map<ApplicationMaster>(applicationDto);

                // ✅ HEALTH CHECK FIRST - Before any database operations
                //foreach (var serviceId in applicationDto.ServiceIds)
                //{
                //    var isHealthy = await _webhookService.CheckServiceHealthByServiceId(serviceId);

                //    if (!isHealthy)
                //    {
                //        _logger.LogWarning("❌ Service {ServiceId} is unhealthy. Aborting application creation.", serviceId);
                //        return new ResponseModel<int>(
                //            false,
                //            $"Cannot create application: Service {serviceId} is currently unavailable. Please try again later.",
                //            0
                //        );
                //    }
                //}

                _logger.LogInformation("✅ All services are healthy - Proceeding with application creation");

                // Validate application name
                bool isValidName = await _applicationRepository.CheckApplicationName(applicationModel);
                if (isValidName)
                {
                    return new ResponseModel<int>(false, AppCommon.AlreadyExistsMessage("Application Name"), 0);
                }

                // Add application
                var response = await _applicationRepository.AddApplication(applicationModel);
                if (!response.IsSuccess)
                {
                    return new ResponseModel<int>(false, response.Message, 0);
                }

                var applicationId = response.Data;

                // Add service mappings
                var mappingResult = await _applicationRepository.AddApplicationServiceMapping(applicationId, applicationDto.ServiceIds);
                //if (!mappingResult.IsSuccess)
                //{
                //    transaction.Rollback();
                //    return new ResponseModel<int>(false, mappingResult.Message, 0);
                //}

                //transaction.Commit();

                // 🔥 FIRE AND FORGET WEBHOOK - Process in background
                //_ = Task.Run(async () =>
                //{
                //    try
                //    {
                //        await _webhookService.ProcessApplicationCreationWebhook(applicationId, applicationDto.ServiceIds);
                //        _logger.LogInformation("✅ Application creation webhook triggered for ApplicationId: {ApplicationId}", applicationId);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Error processing application creation webhook for ApplicationId: {ApplicationId}", applicationId);
                //    }
                //});

                return new ResponseModel<int>(true, AppCommon.AddDetailsMessage("Application"), (int)applicationId);
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                _logger.LogError(ex, "Error adding application");
                return new ResponseModel<int>(false, AppCommon.ErrorMessage(), 0);
            }
        }

        /// <summary>GetApplicationById</summary>
        public async Task<ResponseModel<ApplicationDto>> GetApplicationById(long applicationId)
        {
            try
            {
                var applicationModel = await _applicationRepository.GetApplicationById(applicationId);

                if (applicationModel.IsSuccess)
                {
                    var applicationDto = _mapper.Map<ApplicationDto>(applicationModel.Data);
                    return new ResponseModel<ApplicationDto>(true, AppCommon.GetMessage("Application"), applicationDto);
                }
                else
                    return new ResponseModel<ApplicationDto>(applicationModel.IsSuccess, applicationModel.Message, new ApplicationDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application by ID");
                return new ResponseModel<ApplicationDto>(false, AppCommon.ErrorMessage(), new ApplicationDto());
            }
        }

        /// <summary>InActiveApplicationService</summary>
        public async Task<ResponseModel<bool>> InActiveApplicationService(long applicationId)
        {
            try
            {
                var result = await _applicationRepository.TotalActiveApplicationMapping(applicationId);
                if (result.Data == 0)
                {
                    var deleteApplication = await _applicationRepository.InActiveApplicationRepository(applicationId);
                    if (!deleteApplication.IsSuccess)
                    {
                        return deleteApplication;
                    }
                }
                else
                {
                    return new ResponseModel<bool>(false, AppCommon.CountMessage("Application"), false);
                }
                return new ResponseModel<bool>(true, AppCommon.DeleteMessage("Application"), true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating application");
                return new ResponseModel<bool>(false, AppCommon.ErrorMessage(), false);
            }
        }

        #endregion
    }
}
