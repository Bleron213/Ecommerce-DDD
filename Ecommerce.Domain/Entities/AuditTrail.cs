using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class AuditTrail
    {
        public Guid AuditId { get; set; }
        public AuditType AuditType { get; set; }
        public string AffectedEntity { get; set; } = string.Empty;
        public Guid AffectedEntityId { get; set; }
        public Dictionary<string, object> OldValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string,object>();
        public DateTimeOffset AuditDate { get; set; }
        public string CreatedBy { get; set; } = "SYSTEM";
    }

    public enum AuditType
    {
        NotDefined = 0,
        Added = 1,
        Updated = 2,
        Deleted = 3
    }
}
