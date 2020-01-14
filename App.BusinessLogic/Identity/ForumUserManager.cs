using App.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace App.BusinessLogic.Identity
{
    public class ForumUserManager : UserManager<User>
    {
        public ForumUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
                                IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
                                IEnumerable<IPasswordValidator<User>> passwordValidators,
                                ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
                                IServiceProvider services, ILogger<UserManager<User>> logger) : base(store,
                                                                                                     optionsAccessor,
                                                                                                     passwordHasher,
                                                                                                     userValidators,
                                                                                                     passwordValidators,
                                                                                                     keyNormalizer,
                                                                                                     errors, services,
                                                                                                     logger)
        {

        }
    }
}
