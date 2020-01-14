using App.Data.Entities;
using App.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Data.Contexts
{
    public class ForumDbContextSeed
    {
        private const string pass = "Pass123$";
        private const string adm_role = "Admins";
        private const string mod_role = "GlobalModerators";

        private const string adm_user_name = "admin";
        private const string mod_user_name = "globalmoderator";
        private const string fmod_user_name = "moderator"; // concrete forums moderator
        private const string def_user_name = "bob";
        private const string def_user2_name = "alice";

        private static string adm_user_id = "";
        private static string mod_user_id = "";
        private static string fmod_user_id = "";
        private static string def_user_id = "";
        private static string def_user2_id = "";

        private static UserManager<User> _userManager { get; set; }
        private static RoleManager<Role> _roleManager { get; set; }
        private static ForumDbContext _forumContext { get; set; }

        public static async Task SeedAsync(ForumDbContext forumContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _forumContext = forumContext;
            _userManager = userManager;
            _roleManager = roleManager;

            await UsersSeedAsync();
            await EntitiesSeedAsync();
        }

        private static async Task UsersSeedAsync()
        {
            await _roleManager.CreateAsync(new Role(adm_role));
            await _roleManager.CreateAsync(new Role(mod_role));

            var defaultUser = new User { UserName = def_user_name };
            await _userManager.CreateAsync(defaultUser, pass);
            defaultUser = await _userManager.FindByNameAsync(def_user_name);
            def_user_id = defaultUser.Id;

            var defaultUser2 = new User { UserName = def_user2_name };
            await _userManager.CreateAsync(defaultUser2, pass);
            defaultUser2 = await _userManager.FindByNameAsync(def_user2_name);
            def_user2_id = defaultUser2.Id;

            var fmodUser = new User { UserName = fmod_user_name };
            await _userManager.CreateAsync(fmodUser, pass);
            fmodUser = await _userManager.FindByNameAsync(fmod_user_name);
            fmod_user_id = fmodUser.Id;

            var adminUser = new User { UserName = adm_user_name };
            await _userManager.CreateAsync(adminUser, pass);
            adminUser = await _userManager.FindByNameAsync(adm_user_name);
            await _userManager.AddToRoleAsync(adminUser, adm_role);
            adm_user_id = adminUser.Id;

            var modUser = new User { UserName = mod_user_name };
            await _userManager.CreateAsync(modUser, pass);
            modUser = await _userManager.FindByNameAsync(mod_user_name);
            await _userManager.AddToRoleAsync(modUser, mod_role);
            mod_user_id = modUser.Id;
        }

        private static async Task EntitiesSeedAsync(int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                if (!_forumContext.Forums.Any())
                {
                    _forumContext.Forums.AddRange(
                        GetPreconfiguredForums());

                    await _forumContext.SaveChangesAsync();
                }
                if (!_forumContext.Threads.Any())
                {
                    _forumContext.Threads.AddRange(
                        GetPreconfiguredThreads());

                    await _forumContext.SaveChangesAsync();
                }
                if (!_forumContext.Posts.Any())
                {
                    _forumContext.Posts.AddRange(
                        GetPreconfiguredPosts());

                    await _forumContext.SaveChangesAsync();
                }
                if (!_forumContext.ForumToModerators.Any())
                {
                    _forumContext.ForumToModerators.AddRange(
                        GetPreconfiguredForumToModerators());

                    await _forumContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    // todo: logger
                    // var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    // log.LogError(ex.Message);
                    Console.WriteLine(ex.Message);
                    await EntitiesSeedAsync(retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<Forum> GetPreconfiguredForums()
        {
            return new List<Forum>()
            {
                new Forum() {Name = "Example forum #1 name", Description = "Description of sample forum #1" },
                new Forum() {Name = "Example forum #2 name", Description = "Description of sample forum #2" },
                new Forum() {Name = "Example forum #3 name", Description = "Description of sample forum #3" },
            };
        }

        static IEnumerable<Thread> GetPreconfiguredThreads()
        {
            return new List<Thread>()
            {
                new Thread() {Subject = "Sample Thread Number One", ForumId = 1, UserId = def_user_id},
                new Thread() {Subject = "Second Sample Thread", ForumId = 1, UserId = def_user_id},
                new Thread() {Subject = "Sample Thread Number 3", ForumId = 2, UserId = def_user_id},
                new Thread() {Subject = "Sample Thread Number 4", ForumId = 2, UserId = def_user2_id},
                new Thread() {Subject = "Sample Thread Number 5", ForumId = 3, UserId = def_user2_id},
                new Thread() {Subject = "Sample Thread Number 6", ForumId = 3, UserId = def_user2_id}
            };
        }

        static IEnumerable<Post> GetPreconfiguredPosts()
        {
            return new List<Post>()
            {
                new Post() {Content = "initial post", ThreadId = 1, UserId = def_user_id},
                new Post() {Content = "initial post", ThreadId = 2, UserId = def_user_id},
                new Post() {Content = "initial post", ThreadId = 3, UserId = def_user_id},
                new Post() {Content = "initial post", ThreadId = 4, UserId = def_user2_id},
                new Post() {Content = "initial post", ThreadId = 5, UserId = def_user2_id},
                new Post() {Content = "initial post", ThreadId = 6, UserId = def_user2_id},
            };
        }

        static IEnumerable<ForumToModerator> GetPreconfiguredForumToModerators()
        {
            return new List<ForumToModerator>()
            {
                new ForumToModerator() {ForumId = 2, UserId = fmod_user_id},
                new ForumToModerator() {ForumId = 3, UserId = fmod_user_id},
            };
        }
    }
}
