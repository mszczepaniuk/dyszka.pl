using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class AuditLog : BaseEntity, ICreatedDate, IOwnable
    {
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public Guid? AffectedEntityId { get; set; } 
    }
}
