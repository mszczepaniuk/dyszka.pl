using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Authorization
{
    public static class AuthConstants
    {
        public static string AdminRoleName = "admin";
        public static string ModeratorRoleName = "moderator";

        public static string RoleClaimType = "role";
        public static string UserNameClaimType = "userName";
    }
}
