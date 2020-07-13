using Pl.Dyszka.ApplicationCore.Models.Interfaces;
using System;

namespace Pl.Dyszka.ApplicationCore.Models
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
