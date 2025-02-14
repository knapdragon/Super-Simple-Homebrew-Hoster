using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

public class UserAccountsContext : IdentityDbContext<HomebrewUser>
{
    public UserAccountsContext(DbContextOptions<UserAccountsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new HomebrewUserEntityConfiguration());
    }
}

public class HomebrewUserEntityConfiguration : IEntityTypeConfiguration<HomebrewUser>
{
    public void Configure(EntityTypeBuilder<HomebrewUser> builder)
    {
        builder.Property(x => x.CanMakeBrews).HasDefaultValue(true);
        builder.Property(x => x.CanDeleteBrews).HasDefaultValue(false);
    }
}