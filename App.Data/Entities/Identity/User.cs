using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Data.Entities.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
            Posts = new Collection<Post>();
            Threads = new Collection<Thread>();
            Forums = new Collection<ForumToModerator>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? LastActivityAt { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<UserToRole> Roles { get; set; }
        public virtual ICollection<ForumToModerator> Forums { get; set; }
    }
}
