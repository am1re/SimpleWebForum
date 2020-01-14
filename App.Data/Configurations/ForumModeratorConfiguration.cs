using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations
{
    public class ForumToModeratorConfiguration : IEntityTypeConfiguration<ForumToModerator>
    {
        public void Configure(EntityTypeBuilder<ForumToModerator> builder)
        {
            builder
                .HasKey(r => new { r.UserId, r.ForumId });

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.Forums)
                .HasForeignKey(r => r.UserId);

            builder
                .HasOne(r => r.Forum)
                .WithMany(f => f.Moderators)
                .HasForeignKey(r => r.ForumId);
        }
    }
}
