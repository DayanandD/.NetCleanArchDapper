using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArchDomain.Entities
{
    public class ServiceRegistry
    {
        public long ServiceRegistryID { get; set; }
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string BaseUrl { get; set; }
        public string AddEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
        public string AddonEndpoint { get; set; }
        public bool IsActive { get; set; }
        public bool RequiresAuth { get; set; }
        public int TimeoutSeconds { get; set; }
        public int RetryCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // JSON fields
        public string OperationContext { get; set; } // JSONB in database
        public string PayloadTemplate { get; set; } // JSONB in database

        // Authentication properties (if you want to extend)
        public string AuthType { get; set; } // JWT, API_KEY, HMAC, BASIC
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthToken { get; set; }
    }
}
