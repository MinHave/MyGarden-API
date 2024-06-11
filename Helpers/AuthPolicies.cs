using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGarden_API.API.Helpers
{
    public static class AuthPolicies
    {
        public const string RequireAdmin = "policy-admin";
        public const string RequireManager = "policy-manager";
    }
}
