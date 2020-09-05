using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class IdentityData
    {
        public bool IsBanned { get; set; }
        public List<string> Roles { get; set; }
    }
}
