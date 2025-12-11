using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArchDomain.Entities
{
    public class ServiceSchema
    {
        public long SchemaId { get; set; }
        public long ApplicationId { get; set; }
        public long ServiceId { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string SchemaJson { get; set; } // JSONB in database
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties for display (populated in queries)
        public string ApplicationName { get; set; }
        public string ServiceName { get; set; }
    }
}
