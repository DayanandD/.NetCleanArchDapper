using NETCleanArchDomain.Entities;

namespace NETCleanArchApplication.Interfaces.IRepositories
{
    public interface IUserRepository // Remove IDisposable inheritance if not needed
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<User?> GetByResetTokenAsync(string resetToken);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> EmployeeIdExistsAsync(string employeeId);
    }
}