using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using NETCleanArchApplication.Dtos.Requests;
using NETCleanArchApplication.Dtos.Responses;
using NETCleanArchApplication.Interfaces.IRepositories;
using NETCleanArchApplication.Interfaces.ISecurity;
using NETCleanArchApplication.Interfaces.IServices;
using NETCleanArchInfrastructure.Notifications.IExternalService;

namespace NETCleanArchApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ValidationException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new ValidationException("Account is deactivated");
            }

            user.LastLoginAt = DateTime.UtcNow;
            user.RefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // 7 days refresh token expiry

            await _userRepository.UpdateAsync(user);

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiryMinutes"])),
                User = new UserInfo
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    Department = user.Department,
                    EmployeeId = user.EmployeeId
                }
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new ValidationException("Invalid token"));

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new ValidationException("Invalid refresh token");
            }

            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiryMinutes"])),
                User = new UserInfo
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    Department = user.Department,
                    EmployeeId = user.EmployeeId
                }
            };
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                // Don't reveal whether user exists for security
                return;
            }

            // Generate reset token
            user.ResetPasswordToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(2); // 2 hours expiry

            await _userRepository.UpdateAsync(user);

            // Send email
            var resetLink = $"{_configuration["App:BaseUrl"]}/reset-password?token={user.ResetPasswordToken}";
            var emailBody = $@"
            <h3>Password Reset Request</h3>
            <p>You requested to reset your password. Click the link below to reset your password:</p>
            <a href='{resetLink}'>Reset Password</a>
            <p>This link will expire in 2 hours.</p>
            <p>If you didn't request this, please ignore this email.</p>";

            await _emailService.SendEmailAsync(user.Email, "Password Reset Request", emailBody);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new ValidationException("Passwords do not match");
            }

            var user = await _userRepository.GetByResetTokenAsync(request.Token);
            if (user == null || user.ResetPasswordExpiry < DateTime.UtcNow)
            {
                throw new ValidationException("Invalid or expired reset token");
            }

            // Update password
            CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(token);
                var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");

                var user = await _userRepository.GetByIdAsync(userId);
                return user != null && user.IsActive;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _userRepository.UpdateAsync(user);
            }
        }

        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(Convert.FromBase64String(storedHash));
        }
    }
}
