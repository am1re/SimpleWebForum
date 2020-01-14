using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations
{
    public class ThreadConfiguration : IEntityTypeConfiguration<Thread>
    {
        public void Configure(EntityTypeBuilder<Thread> builder)
        {
            builder
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder
                .Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(64);
        }
    }
}
