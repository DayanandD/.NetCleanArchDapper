namespace NETCleanArchApplication.Dtos
{
    /// <summary>
    /// DTO for Application Service Registry Schema
    /// </summary>
    public class ApplicationServiceRegistrySchemaDto
    {
        public long schemasid { get; set; }
        public string schemaname { get; set; }
        public long serviceregistryid { get; set; }
        public long applicationid { get; set; }
        public long serviceid { get; set; }
        public string endpoint { get; set; }
        public string method { get; set; }
        public string schemajson { get; set; }
        public int version { get; set; }
        public bool isactive { get; set; }
        public string createdby { get; set; }
        public DateTime createdon { get; set; }
        public string updatedby { get; set; }
        public DateTime? updatedon { get; set; }
        public string baseurl { get; set; }

        // Display properties
        public string applicationname { get; set; }
        public string servicename { get; set; }
    }
}