using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class ApplicationUser : BaseEntity
    {
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}
