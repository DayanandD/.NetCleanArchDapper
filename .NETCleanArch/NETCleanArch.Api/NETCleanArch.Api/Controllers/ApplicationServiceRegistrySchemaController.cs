using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCleanArchApplication.Dtos;
using NETCleanArchApplication.Dtos.Common;
using NETCleanArchApplication.IServices;

namespace NETCleanArchApi.Controllers
{
    /// <summary>
    /// Controller for managing Application Service Registry Schema.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServiceRegistrySchemaController : ControllerBase
    {
        private readonly IApplicationServiceRegistrySchemaService _applicationServiceRegistrySchemaService;
        private readonly ILogger<ApplicationServiceRegistrySchemaController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationServiceRegistrySchemaController"/> class.
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaService">Service for handling application service registry schema operations.</param>
        /// <param name="logger">Logger instance for logging.</param>
        public ApplicationServiceRegistrySchemaController(
            IApplicationServiceRegistrySchemaService applicationServiceRegistrySchemaService,
            ILogger<ApplicationServiceRegistrySchemaController> logger)
        {
            _applicationServiceRegistrySchemaService = applicationServiceRegistrySchemaService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the list of application service registry schemas.
        /// </summary>
        /// <returns>A list of application service registry schemas.</returns>
        [HttpGet("GetApplicationServiceRegistrySchemaList")]
        public async Task<IActionResult> GetApplicationServiceRegistrySchemaList([FromQuery] PaginationRequest paginationRequest)
        {
            try
            {
                var response = await _applicationServiceRegistrySchemaService.GetApplicationServiceRegistrySchemaListService(paginationRequest);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Add or Update Application Service Registry Schema
        /// </summary>
        /// <param name="applicationServiceRegistrySchemaDto"></param>
        /// <returns></returns>
        [HttpPost("AddApplicationServiceRegistrySchema")]
        public async Task<IActionResult> AddApplicationServiceRegistrySchema([FromBody] ApplicationServiceRegistrySchemaDto applicationServiceRegistrySchemaDto)
        {
            try
            {
                var response = await _applicationServiceRegistrySchemaService.AddApplicationServiceRegistrySchemaService(applicationServiceRegistrySchemaDto);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Get Application Service Registry Schema Detail
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        [HttpGet("GetApplicationServiceRegistrySchemaDetail/{schemasid}")]
        public async Task<IActionResult> GetApplicationServiceRegistrySchemaDetail(long schemasid)
        {
            try
            {
                if (schemasid <= 0)
                    return BadRequest(new ResponseModel<string>(false, AppCommon.InvalidMessage("Schema Id"), ""));

                var response = await _applicationServiceRegistrySchemaService.GetApplicationServiceRegistrySchemaDetailService(schemasid);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Deletes a specific application service registry schema.
        /// </summary>
        /// <param name="schemasid">The ID of the schema to delete.</param>
        /// <returns>A success message if the schema is deleted.</returns>
        [HttpDelete("DeleteApplicationServiceRegistrySchema/{schemasid}")]
        public async Task<IActionResult> DeleteApplicationServiceRegistrySchema(long schemasid)
        {
            try
            {
                if (schemasid <= 0)
                    return BadRequest(new ResponseModel<string>(false, AppCommon.InvalidMessage("Schema Id"), ""));

                var response = await _applicationServiceRegistrySchemaService.DeleteApplicationServiceRegistrySchemaService(schemasid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// InActive Application Service Registry Schema
        /// </summary>
        /// <param name="schemasid"></param>
        /// <returns></returns>
        [HttpDelete("InActiveApplicationServiceRegistrySchema/{schemasid}")]
        public async Task<IActionResult> InActiveApplicationServiceRegistrySchema(long schemasid)
        {
            try
            {
                if (schemasid <= 0)
                    return BadRequest(new ResponseModel<string>(false, AppCommon.InvalidMessage("Schema Id"), ""));

                var response = await _applicationServiceRegistrySchemaService.InActiveApplicationServiceRegistrySchemaService(schemasid);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Get schemas by Application and Service
        /// </summary>
        /// <param name="applicationid"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        [HttpGet("GetSchemasByApplicationService/{applicationid}/{serviceid}")]
        public async Task<IActionResult> GetSchemasByApplicationService(long applicationid, long serviceid)
        {
            try
            {
                if (applicationid <= 0 || serviceid <= 0)
                    return BadRequest(new ResponseModel<string>(false, "Invalid Application Id or Service Id", ""));

                var response = await _applicationServiceRegistrySchemaService.GetSchemasByApplicationServiceService(applicationid, serviceid);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }
    }
}