using Dapper;
using System.Data;
using NETCleanArchDomain.Entities;
using NETCleanArch.Application.Interfaces.IRepositories;
using NETCleanArchInfrastructure.Data;

namespace NETCleanArchInfrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperDbContext _dbContext;

        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"SELECT * FROM NETCleanArchusermaster WHERE UserId = @UserId AND IsActive = true";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"SELECT * FROM NETCleanArchusermaster WHERE Email = @Email AND IsActive = true";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"SELECT * FROM NETCleanArchusermaster WHERE RefreshToken = @RefreshToken AND IsActive = true";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { RefreshToken = refreshToken });
        }

        public async Task<User?> GetByResetTokenAsync(string resetToken)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"SELECT * FROM NETCleanArchusermaster WHERE ResetPasswordToken = @ResetToken AND IsActive = true";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { ResetToken = resetToken });
        }

        public async Task AddAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"INSERT INTO NETCleanArchusermaster 
                            (UserId, FirstName, LastName, Email, PasswordHash, PasswordSalt, PhoneNumber, 
                             Department, EmployeeId, ExtNumber, Role, IsActive, CreatedAt) 
                            VALUES 
                            (@UserId, @FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt, @PhoneNumber,
                             @Department, @EmployeeId, @ExtNumber, @Role, @IsActive, @CreatedAt)";
            await connection.ExecuteAsync(sql, user);
        }

        public async Task UpdateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"UPDATE NETCleanArchusermaster SET 
                            FirstName = @FirstName, LastName = @LastName, Email = @Email,
                            PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, 
                            PhoneNumber = @PhoneNumber, Department = @Department, EmployeeId = @EmployeeId,
                            ExtNumber = @ExtNumber, Role = @Role, IsActive = @IsActive, 
                            UpdatedAt = @UpdatedAt, LastLoginAt = @LastLoginAt,
                            RefreshToken = @RefreshToken, RefreshTokenExpiry = @RefreshTokenExpiry,
                            ResetPasswordToken = @ResetPasswordToken, ResetPasswordExpiry = @ResetPasswordExpiry
                            WHERE UserId = @UserId";
            await connection.ExecuteAsync(sql, user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"SELECT CASE WHEN EXISTS(SELECT 1 FROM NETCleanArchusermaster WHERE Email = @Email) THEN 1 ELSE 0 END";
            var exists = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
            return exists == 1;
        }

        public async Task<bool> EmployeeIdExistsAsync(string employeeId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"SELECT CASE WHEN EXISTS(SELECT 1 FROM NETCleanArchusermaster WHERE EmployeeId = @EmployeeId) THEN 1 ELSE 0 END";
            var exists = await connection.ExecuteScalarAsync<int>(sql, new { EmployeeId = employeeId });
            return exists == 1;
        }

        // Remove Dispose method since we're creating connections per operation
        // public void Dispose()
        // {
        //     _connection?.Dispose();
        // }
    }
}