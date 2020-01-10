using CoolTool.Entity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolTool.DataAccess.DbContexts
{
    internal class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasOne(x => x.Company)
                .WithMany(z => z.Users).IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.IdentityUser);
        }
    }

    internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasMany(x => x.Users)
                .WithOne(z => z.Company).IsRequired(true)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
