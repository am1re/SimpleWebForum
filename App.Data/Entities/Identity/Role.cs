using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace App.Data.Entities.Identity
{
    public class Role : IdentityRole
    {
        public Role(string roleName) : base(roleName)
        {
        }
        public Role() : base()
        {

        }
        public virtual ICollection<UserToRole> UserRoles { get; set; }
    }
}
