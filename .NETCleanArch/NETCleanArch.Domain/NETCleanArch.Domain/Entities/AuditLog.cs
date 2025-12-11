namespace NETCleanArchDomain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string? TableName { get; set; }
        public string? Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
    }
}
