using App.BusinessLogic.Resources.Identity.Role;
using System;
using System.Collections.Generic;

namespace App.BusinessLogic.Resources.Identity.User
{
    public class UserResource
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int PostsCount { get; set; }
        public int ThreadsCount { get; set; }

        public DateTime RegisteredAt { get; set; }
        public DateTime? LastActivityAt { get; set; }

        public ICollection<RoleResource> Roles { get; set; }

        // signature ?
        // last post ?
        //public PostModel LastPost { get; set; }
        //public string AvatarUrl { get; set; }
    }
}
