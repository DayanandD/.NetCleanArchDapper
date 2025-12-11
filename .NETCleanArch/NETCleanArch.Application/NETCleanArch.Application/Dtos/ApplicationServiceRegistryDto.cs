namespace NETCleanArch.Application.Dtos
{
    /// <summary>
    /// DTO for Application Service Registry
    /// </summary>
    public class ApplicationServiceRegistryDto
    {
        public long serviceregistryid { get; set; }
        public long serviceid { get; set; }
        public string servicename { get; set; }
        public string baseurl { get; set; }
        public string addendpoint { get; set; }
        public string updateendpoint { get; set; }
        public string deleteendpoint { get; set; }
        public string addonendpoint { get; set; }
        public bool isactive { get; set; }
        public bool requiresauth { get; set; }
        public int timeoutseconds { get; set; }
        public int retrycount { get; set; }
        public DateTime createdat { get; set; }
        public DateTime updatedat { get; set; }
        public string operationcontext { get; set; }
        public string payloadtemplate { get; set; }
    }
}