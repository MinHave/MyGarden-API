using System.Collections.Generic;

namespace MyGarden_API.API.Helpers
{
    public static class AuthRoles
    {
        public const string Admin = "admin";
        public const string Manager = "manager";

        public static IReadOnlyList<string> List { get; }
                = new List<string>()
                {
                    Admin, Manager
                };
    }
}
