using System;
using System.Collections.Generic;

namespace App.API.Models.User
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int PostsCount { get; set; }
        public int ThreadsCount { get; set; }

        public DateTime RegisteredAt { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
