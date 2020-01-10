using CoolTool.Entity.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoolTool.DataAccess.DbContexts
{
    public sealed class IdentityDbContext : IdentityDbContext<IdentityUser, Role, long>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new IdentityUserEntityConfiguration());
        }
    }
}
