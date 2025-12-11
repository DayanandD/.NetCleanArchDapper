using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArch.Application.Dtos
{
    public class ServiceRegistryDto
    {
        public long ServiceRegistryId { get; set; }
        public long? ServiceId { get; set; }
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
        public string OperationContext { get; set; } // JSON string
        public string PayloadTemplate { get; set; } // JSON string
    }
}
