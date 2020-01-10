using Microsoft.EntityFrameworkCore;

namespace CoolTool.QueueProvider.DataAccess
{
    public class QueueDbContext : DbContext
    {
        public DbSet<Queue> Queues { get; set; }
        public DbSet<SystemEventSetting> EventSettings { get; set; }
        public DbSet<QueueMessageError> QueueMessageErrors { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new QueueConfiguration());
        }

    }
}
