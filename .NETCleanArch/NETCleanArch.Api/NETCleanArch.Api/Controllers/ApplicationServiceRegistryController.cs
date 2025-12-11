//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using NETCleanArchApplication.Dtos;
//using NETCleanArchApplication.Dtos.Common;
//using NETCleanArchApplication.Interfaces.IRepositories;
//using NETCleanArchApplication.Interfaces.IServices;

//namespace NETCleanArchApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    public class ApplicationServiceRegistryController : ControllerBase
//    {
//        #region Constructor
//        private readonly IServiceRegistryRepository _applicationServiceRegistryService;
//        private readonly ILogger<ApplicationServiceRegistryController> _logger;

//        public ApplicationServiceRegistryController(
//            IServiceRegistryRepository applicationServiceRegistryService,
//            ILogger<ApplicationServiceRegistryController> logger)
//        {
//            _applicationServiceRegistryService = applicationServiceRegistryService;
//            _logger = logger;
//        }
//        #endregion

//        #region Service Registry Endpoints

//        /// <summary>
//        /// Get all service registries
//        /// </summary>
//        [HttpGet("registry")]
//        public async Task<IActionResult> GetAllServiceRegistry([FromQuery] PaginationRequest paginationRequest)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.GetAllServiceRegistry(paginationRequest);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in GetAllServiceRegistry");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Add new service registry
//        /// </summary>
//        [HttpPost("registry")]
//        public async Task<IActionResult> AddServiceRegistry([FromBody] ServiceRegistryDto serviceRegistryDto)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.AddServiceRegistry(serviceRegistryDto);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in AddServiceRegistry");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Get service registry by ID
//        /// </summary>
//        [HttpGet("registry/{serviceRegistryId}")]
//        public async Task<IActionResult> GetServiceRegistryById(long serviceRegistryId)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.GetServiceRegistryById(serviceRegistryId);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in GetServiceRegistryById");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Update service registry
//        /// </summary>
//        [HttpPut("registry")]
//        public async Task<IActionResult> UpdateServiceRegistry([FromBody] ServiceRegistryDto serviceRegistryDto)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.UpdateServiceRegistry(serviceRegistryDto);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in UpdateServiceRegistry");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Delete service registry
//        /// </summary>
//        [HttpDelete("registry/{serviceRegistryId}")]
//        public async Task<IActionResult> DeleteServiceRegistry(long serviceRegistryId)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.DeleteServiceRegistry(serviceRegistryId);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in DeleteServiceRegistry");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        #endregion

//        #region Service Schema Endpoints

//        /// <summary>
//        /// Get all service schemas
//        /// </summary>
//        [HttpGet("schema")]
//        public async Task<IActionResult> GetAllServiceSchemas([FromQuery] PaginationRequest paginationRequest)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.GetAllServiceSchemas(paginationRequest);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in GetAllServiceSchemas");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Get schemas by application ID
//        /// </summary>
//        [HttpGet("schema/application/{applicationId}")]
//        public async Task<IActionResult> GetSchemasByApplicationId(long applicationId, [FromQuery] PaginationRequest paginationRequest)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.GetSchemasByApplicationId(applicationId, paginationRequest);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in GetSchemasByApplicationId");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Get schemas by service ID
//        /// </summary>
//        [HttpGet("schema/service/{serviceId}")]
//        public async Task<IActionResult> GetSchemasByServiceId(long serviceId, [FromQuery] PaginationRequest paginationRequest)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.GetSchemasByServiceId(serviceId, paginationRequest);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in GetSchemasByServiceId");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Add new service schema
//        /// </summary>
//        [HttpPost("schema")]
//        public async Task<IActionResult> AddServiceSchema([FromBody] ApplicationServiceRegistryDto.ServiceSchemaDto serviceSchemaDto)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.AddServiceSchema(serviceSchemaDto);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in AddServiceSchema");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Get service schema by ID
//        /// </summary>
//        [HttpGet("schema/{schemaId}")]
//        public async Task<IActionResult> GetServiceSchemaById(long schemaId)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.GetServiceSchemaById(schemaId);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in GetServiceSchemaById");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Update service schema
//        /// </summary>
//        [HttpPut("schema")]
//        public async Task<IActionResult> UpdateServiceSchema([FromBody] ApplicationServiceRegistrySchemaDto serviceSchemaDto)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.UpdateServiceSchema(serviceSchemaDto);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in UpdateServiceSchema");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        /// <summary>
//        /// Delete service schema
//        /// </summary>
//        [HttpDelete("schema/{schemaId}")]
//        public async Task<IActionResult> DeleteServiceSchema(long schemaId)
//        {
//            try
//            {
//                var response = await _applicationServiceRegistryService.DeleteServiceSchema(schemaId);
//                if (response.IsSuccess)
//                    return Ok(response);
//                else
//                    return StatusCode(500, response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in DeleteServiceSchema");
//                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
//            }
//        }

//        #endregion
//    }
//}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCleanArchApplication.Dtos;
using NETCleanArchApplication.Dtos.Common;
using NETCleanArchApplication.Interfaces.IServices;

namespace NETCleanArchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationServiceRegistryController : ControllerBase
    {
        #region Constructor
        private readonly IApplicationServiceRegistryService _applicationServiceRegistryService;
        private readonly ILogger<ApplicationServiceRegistryController> _logger;

        public ApplicationServiceRegistryController(
            IApplicationServiceRegistryService applicationServiceRegistryService,
            ILogger<ApplicationServiceRegistryController> logger)
        {
            _applicationServiceRegistryService = applicationServiceRegistryService;
            _logger = logger;
        }
        #endregion

        #region Service Registry Endpoints

        /// <summary>
        /// Get all service registries
        /// </summary>
        [HttpGet("registry")]
        public async Task<IActionResult> GetAllServiceRegistry([FromQuery] PaginationRequest paginationRequest)
        {
            try
            {
                var response = await _applicationServiceRegistryService.GetAllServiceRegistry(paginationRequest);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllServiceRegistry");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Add new service registry
        /// </summary>
        [HttpPost("registry")]
        public async Task<IActionResult> AddServiceRegistry([FromBody] ServiceRegistryDto serviceRegistryDto)
        {
            try
            {
                var response = await _applicationServiceRegistryService.AddServiceRegistry(serviceRegistryDto);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddServiceRegistry");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Get service registry by ID
        /// </summary>
        [HttpGet("registry/{serviceRegistryId}")]
        public async Task<IActionResult> GetServiceRegistryById(long serviceRegistryId)
        {
            try
            {
                var response = await _applicationServiceRegistryService.GetServiceRegistryById(serviceRegistryId);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceRegistryById");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Update service registry
        /// </summary>
        [HttpPut("registry")]
        public async Task<IActionResult> UpdateServiceRegistry([FromBody] ServiceRegistryDto serviceRegistryDto)
        {
            try
            {
                var response = await _applicationServiceRegistryService.UpdateServiceRegistry(serviceRegistryDto);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateServiceRegistry");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Delete service registry
        /// </summary>
        [HttpDelete("registry/{serviceRegistryId}")]
        public async Task<IActionResult> DeleteServiceRegistry(long serviceRegistryId)
        {
            try
            {
                var response = await _applicationServiceRegistryService.DeleteServiceRegistry(serviceRegistryId);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceRegistry");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        #endregion

        #region Service Schema Endpoints

        /// <summary>
        /// Get all service schemas
        /// </summary>
        [HttpGet("schema")]
        public async Task<IActionResult> GetAllServiceSchemas([FromQuery] PaginationRequest paginationRequest)
        {
            try
            {
                var response = await _applicationServiceRegistryService.GetAllServiceSchemas(paginationRequest);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllServiceSchemas");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Get schemas by application ID
        /// </summary>
        [HttpGet("schema/application/{applicationId}")]
        public async Task<IActionResult> GetSchemasByApplicationId(long applicationId, [FromQuery] PaginationRequest paginationRequest)
        {
            try
            {
                var response = await _applicationServiceRegistryService.GetSchemasByApplicationId(applicationId, paginationRequest);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSchemasByApplicationId");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Get schemas by service ID
        /// </summary>
        [HttpGet("schema/service/{serviceId}")]
        public async Task<IActionResult> GetSchemasByServiceId(long serviceId, [FromQuery] PaginationRequest paginationRequest)
        {
            try
            {
                var response = await _applicationServiceRegistryService.GetSchemasByServiceId(serviceId, paginationRequest);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSchemasByServiceId");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Add new service schema
        /// </summary>
        [HttpPost("schema")]
        public async Task<IActionResult> AddServiceSchema([FromBody] ServiceSchemaDto serviceSchemaDto)
        {
            try
            {
                var response = await _applicationServiceRegistryService.AddServiceSchema(serviceSchemaDto);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddServiceSchema");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Get service schema by ID
        /// </summary>
        [HttpGet("schema/{schemaId}")]
        public async Task<IActionResult> GetServiceSchemaById(long schemaId)
        {
            try
            {
                var response = await _applicationServiceRegistryService.GetServiceSchemaById(schemaId);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetServiceSchemaById");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Update service schema
        /// </summary>
        [HttpPut("schema")]
        public async Task<IActionResult> UpdateServiceSchema([FromBody] ServiceSchemaDto serviceSchemaDto)
        {
            try
            {
                var response = await _applicationServiceRegistryService.UpdateServiceSchema(serviceSchemaDto);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateServiceSchema");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        /// <summary>
        /// Delete service schema
        /// </summary>
        [HttpDelete("schema/{schemaId}")]
        public async Task<IActionResult> DeleteServiceSchema(long schemaId)
        {
            try
            {
                var response = await _applicationServiceRegistryService.DeleteServiceSchema(schemaId);
                if (response.IsSuccess)
                    return Ok(response);
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteServiceSchema");
                return StatusCode(500, new ResponseModel<string>(false, AppCommon.ErrorMessage(), ""));
            }
        }

        #endregion
    }
}