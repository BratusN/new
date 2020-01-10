using CoolTool.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolTool.DataAccess.DbContexts
{
    internal class QueueConfiguration : IEntityTypeConfiguration<Queue>
    {
        public void Configure(EntityTypeBuilder<Queue> builder)
        {
            builder.HasMany(x => x.Events)
                .WithOne(z => z.Queue)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
