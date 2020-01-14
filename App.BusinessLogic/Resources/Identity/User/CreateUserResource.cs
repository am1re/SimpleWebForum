using AutoMapper.Configuration.Annotations;

namespace App.BusinessLogic.Resources.Identity.User
{
    public class CreateUserResource
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Ignore]
        public string Password { get; set; }
    }
}
