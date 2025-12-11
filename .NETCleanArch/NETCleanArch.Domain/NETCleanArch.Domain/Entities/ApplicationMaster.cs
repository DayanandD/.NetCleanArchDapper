using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCleanArchDomain.Entities
{
    public class ApplicationMaster
    {
        public long ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation property for services
        public string Services { get; set; }
    }
}
