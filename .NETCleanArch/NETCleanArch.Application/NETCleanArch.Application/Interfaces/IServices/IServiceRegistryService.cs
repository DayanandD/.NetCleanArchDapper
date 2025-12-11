using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;

namespace NETCleanArch.Application.Interfaces.IServices
{
    public interface IServiceRegistryService
    {
        // Service Registry Operations
        Task<ResponseModel<List<ServiceRegistryDto>>> GetAllServiceRegistry(PaginationRequest paginationRequest);
        Task<ResponseModel<long>> AddServiceRegistry(ServiceRegistryDto serviceRegistryDto);
        Task<ResponseModel<ServiceRegistryDto>> GetServiceRegistryById(long serviceRegistryId);
        Task<ResponseModel<bool>> UpdateServiceRegistry(ServiceRegistryDto serviceRegistryDto);
        Task<ResponseModel<bool>> DeleteServiceRegistry(long serviceRegistryId);

        // Service Schema Operations
        Task<ResponseModel<List<ServiceSchemaDto>>> GetAllServiceSchemas(PaginationRequest paginationRequest);
        Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByApplicationId(long applicationId, PaginationRequest paginationRequest);
        Task<ResponseModel<List<ServiceSchemaDto>>> GetSchemasByServiceId(long serviceId, PaginationRequest paginationRequest);
        Task<ResponseModel<long>> AddServiceSchema(ServiceSchemaDto serviceSchemaDto);
        Task<ResponseModel<ServiceSchemaDto>> GetServiceSchemaById(long schemaId);
        Task<ResponseModel<bool>> UpdateServiceSchema(ServiceSchemaDto serviceSchemaDto);
        Task<ResponseModel<bool>> DeleteServiceSchema(long schemaId);
    }
}
