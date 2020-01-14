using App.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.FirstName)
                .HasMaxLength(32);

            builder
                .Property(u => u.LastName)
                .HasMaxLength(32);

            builder
                .Property(u => u.RegisteredAt)
                .HasDefaultValueSql("getdate()");

            builder.HasMany(u => u.Roles)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired();
        }
    }
}
