using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArchApplication.Dtos
{
    public class ServiceSchemaDto
    {
        public long SchemaId { get; set; }
        public long ApplicationId { get; set; }
        public long ServiceId { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string SchemaJson { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ApplicationName { get; set; }
        public string ServiceName { get; set; }
    }

    public class ServiceSchemaFieldDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public bool Required { get; set; }
        public string Default { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
