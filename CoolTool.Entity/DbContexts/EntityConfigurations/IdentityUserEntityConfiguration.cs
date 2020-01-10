using System;
using System.Collections.Generic;
using System.Text;
using CoolTool.Entity.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolTool.DataAccess.DbContexts
{
    internal class IdentityUserEntityConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.Ignore(c => c.LockoutEnabled);
            builder.Ignore(c => c.AccessFailedCount);
            builder.Ignore(c => c.NormalizedEmail);
            builder.Ignore(c => c.NormalizedUserName);
            builder.Ignore(c => c.ConcurrencyStamp);
            builder.Ignore(c => c.LockoutEnd);
            builder.Ignore(c => c.LockoutEnabled);
        }
    }
}
