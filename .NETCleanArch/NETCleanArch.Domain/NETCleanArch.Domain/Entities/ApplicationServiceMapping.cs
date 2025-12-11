using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArchDomain.Entities
{
    public class ApplicationServiceMapping
    {
        public long AppServiceMappingId { get; set; }
        public long ApplicationId { get; set; }
        public long ServiceId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
