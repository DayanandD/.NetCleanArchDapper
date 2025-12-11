using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;
using NETCleanArch.Application.Interfaces.IServices;

namespace NETCleanArchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationController : ControllerBase
    {
        #region Constructor
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IApplicationService applicationService, ILogger<ApplicationController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }
        #endregion

        #region Application Master

        /// <summary>
        /// GetAllApplication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllApplication([FromQuery] PaginationRequest paginationRequest)
        {
            try
            {
                var response = await _applicationService.GetAllApplication(paginationRequest);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllApplication");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// AddApplication
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddApplication([FromBody] ApplicationDto applicationDto)
        {
            try
            {
                var response = await _applicationService.AddApplication(applicationDto);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddApplication");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// GetApplicationById
        /// </summary>
        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetApplicationById(long applicationId)
        {
            try
            {
                var response = await _applicationService.GetApplicationById(applicationId);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetApplicationById");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// InActiveApplication
        /// </summary>
        [HttpDelete("{applicationId}")]
        public async Task<IActionResult> InActiveApplication(long applicationId)
        {
            try
            {
                var response = await _applicationService.InActiveApplicationService(applicationId);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InActiveApplication");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// GetApplicationServiceMapping
        /// </summary>
        [HttpGet("{applicationId}/services")]
        public async Task<IActionResult> GetApplicationServiceIds(long applicationId)
        {
            try
            {
                // This would call a repository method to get service IDs
                // For now returning placeholder
                return Ok(new { applicationId, serviceIds = new int[] { } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetApplicationServiceIds");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        #endregion
    }
}
