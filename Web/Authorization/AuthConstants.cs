using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Authorization
{
    public static class AuthConstants
    {
        public const string AdminRoleName = "admin";
        public const string ModeratorRoleName = "moderator";

        public const string UserNameClaimType = "userName";
        public const string IsBannedClaimType = "isBanned";

        public const string OnlyAdminPolicy = "OnlyAdmin";
        public const string OnlyModeratorPolicy = "OnlyModerator";
        public const string ModeratorOrAdminPolicy = "ModeratorOrAdmin";
        public const string UserRemovalPolicy = "UserRemoval";
        public const string ProfileOwnerPolicy = "ProfileOwner";
        public const string NotBannedPolicy = "NotBanned";
        public const string EntityOwnerPolicy = "EntityOwner";
    }
}
