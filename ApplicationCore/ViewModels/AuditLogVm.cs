using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ViewModels
{
    public class AuditLogVm
    {
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorUsername { get; set; }
        public Guid? AffectedEntityId { get; set; }
    }
}
