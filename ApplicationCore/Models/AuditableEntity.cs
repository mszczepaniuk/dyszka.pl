using System;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class AuditableEntity : BaseEntity, IOwnable, ICreatedDate, IUpdatedBy, IUpdatedDate
    {
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}