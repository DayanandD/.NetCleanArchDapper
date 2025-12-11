namespace NETCleanArchApplication.Dtos
{
    public class ApplicationDto
    {
        public long ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int[] ServiceIds { get; set; }
        public string Services { get; set; }
    }
}
