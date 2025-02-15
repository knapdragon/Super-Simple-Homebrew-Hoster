using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

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

/// <summary>
///     Credit to Steve Gordon (2016) for a guide to overriding
///     https://www.stevejgordon.co.uk/extending-the-asp-net-core-identity-signinmanager
/// </summary>
public class HomebrewSignInManager<HomebrewUser>(UserManager<HomebrewUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<HomebrewUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<HomebrewUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<HomebrewUser> confirmation)
    : SignInManager<HomebrewUser>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) where HomebrewUser : class
{
    /// <summary>
    ///     Overrides base SignInManager's PasswordSignInAsync to use email instead of username.
    ///     This was implemented because by default, Identity authentication sets UserName to a user's email as well.
    ///     If you want to have custom usernames without overriding this method, logging in causes invalid login attempts as it tries to find the user by userName when it should be comparing email.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
    /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
    /// <returns></returns>
    public override async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
    {
        var user = await UserManager.FindByEmailAsync(email);
        if (user == null) {
            return SignInResult.Failed;
        }

        return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
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