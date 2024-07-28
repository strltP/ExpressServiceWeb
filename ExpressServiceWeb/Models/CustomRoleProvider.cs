using ExpressServiceWeb.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace ExpressServiceWeb
{
    public class CustomRoleProvider : RoleProvider
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();

        public override string[] GetRolesForUser(string username)
        {
            var user = db.Users.FirstOrDefault(u => u.email == username);
            if (user != null)
            {
                return new[] { user.role };
            }
            return new string[] { };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = db.Users.FirstOrDefault(u => u.email == username);
            if (user != null)
            {
                return user.role == roleName;
            }
            return false;
        }

        // Implement other methods as needed
        public override void CreateRole(string roleName) { throw new NotImplementedException(); }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotImplementedException(); }
        public override bool RoleExists(string roleName) { throw new NotImplementedException(); }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }
        public override string[] GetUsersInRole(string roleName) { throw new NotImplementedException(); }
        public override string[] GetAllRoles() { throw new NotImplementedException(); }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch) { throw new NotImplementedException(); }
        public override string ApplicationName { get; set; }
    }
}

