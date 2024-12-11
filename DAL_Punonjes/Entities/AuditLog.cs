using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.Entities
{
    public enum AuditLogType
    {
        Create,
        Update,
        Delete
    }
    public class AuditLog
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public AuditLogType LogType { get; set; }
        public string Details { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
