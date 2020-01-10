using CoolTool.Entity.Identity;
using CoolTool.Entity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Action = CoolTool.Entity.User.Action;

namespace CoolTool.DataAccess.DbContexts
{
    public sealed class UserServiceDbContext : IdentityDbContext<IdentityUser, Role, long>
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Application> Applications { get; set; }

        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new IdentityUserEntityConfiguration());
            builder.ApplyConfiguration(new ApplicationConfiguration());
        }
    }
}
