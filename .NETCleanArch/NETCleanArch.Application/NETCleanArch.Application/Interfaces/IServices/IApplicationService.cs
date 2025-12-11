using NETCleanArch.Application.Dtos;
using NETCleanArch.Application.Dtos.Common;

namespace NETCleanArch.Application.Interfaces.IServices
{
    public interface IApplicationService
    {
        /// <summary>
        /// GetAllApplication
        /// </summary>
        Task<ResponseModel<List<ApplicationDto>>> GetAllApplication(PaginationRequest paginationRequest);

        /// <summary>
        /// AddApplication
        /// </summary>
        Task<ResponseModel<int>> AddApplication(ApplicationDto applicationDto);

        /// <summary>
        /// GetApplicationById
        /// </summary>
        Task<ResponseModel<ApplicationDto>> GetApplicationById(long applicationId);

        /// <summary>
        /// InActiveApplicationService
        /// </summary>
        Task<ResponseModel<bool>> InActiveApplicationService(long applicationId);
    }
}
