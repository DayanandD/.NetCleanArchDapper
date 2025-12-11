namespace NETCleanArchInfrastructure.Notifications.IExternalService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
