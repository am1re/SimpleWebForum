using App.Data.Configurations;
using App.Data.Configurations.Identity;
using App.Data.Entities;
using App.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Contexts
{
    public class ForumDbContext : IdentityDbContext
        <User, Role, string, IdentityUserClaim<string>, UserToRole,
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ForumToModerator> ForumToModerators { get; set; }

        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());

            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new ForumConfiguration());
            builder.ApplyConfiguration(new ThreadConfiguration());
            builder.ApplyConfiguration(new ForumToModeratorConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
