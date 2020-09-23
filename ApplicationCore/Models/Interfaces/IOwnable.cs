using System;

namespace ApplicationCore.Models.Interfaces
{
    public interface IOwnable
    {
        public ApplicationUser CreatedBy { get; set; }
    }
}