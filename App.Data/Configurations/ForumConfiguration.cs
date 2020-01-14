using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations
{
    public class ForumConfiguration : IEntityTypeConfiguration<Forum>
    {
        public void Configure(EntityTypeBuilder<Forum> builder)
        {
            builder
                .Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(64);

            builder
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder
                .Property(f => f.IsActive)
                .ValueGeneratedNever();
        }
    }
}
