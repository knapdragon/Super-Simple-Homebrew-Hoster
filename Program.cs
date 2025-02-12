using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models;
using Super_Simple_Homebrew_Hoster.Data;
using Super_Simple_Homebrew_Hoster.Models;
using Microsoft.AspNetCore.Identity;
using Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

namespace Super_Simple_Homebrew_Hoster
{ 
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add database context for primary homebrew item database
            builder.Services.AddDbContext<Super_Simple_Homebrew_HosterContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Super_Simple_Homebrew_HosterContext") ?? throw new InvalidOperationException("Connection string 'Super_Simple_Homebrew_HosterContext' not found.")));

            // Add database context for identity services
            builder.Services.AddDbContext<UserAccountsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Super_Simple_Homebrew_HosterContext") ?? throw new InvalidOperationException("Connection string 'Super_Simple_Homebrew_HosterContext' not found.")));
            
            // .AddDefaultIdentity<>(): Create a new default identity service based on HomebrewUser type; confirmed account is required to use the application
            // .AddRoles<>(): Create default role services based on IdentityRole, which is good enough for our purposes
            // .AddEntityFrameworkStores<>(): Set entity framework implementation to the respective context, required for identity functionality
            builder.Services.AddDefaultIdentity<HomebrewUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserAccountsContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Initialisation of roles
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;   // Easier referencing

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); // Get roleManager service with the IdentityRole type
                var roles = new[] { "Admin", "User", "Guest" };     // Create default roles

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));  // Create role only if it doesn't exist
                    }
                }

                SeedData.Initialize(services);
            }

            // Initilisation of admin user and assignment of role
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;   // Easier referencing

                var userManager = services.GetRequiredService<UserManager<HomebrewUser>>(); // Get userManager service with custom HomebrewUser type, for improved customisation

                string adminEmail = "admin@admin.com";  // placeholder; going to want improved security, of course
                string adminPassword = "Test1234,";     // placeholder; going to want improved security, of course

                // If the admin user's email doesn't exist
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    // Creates a new user with appropriate permissions for an admin
                    var user = new HomebrewUser
                    {
                        DisplayName = "Admin",
                        CanMakeBrews = true,
                        CanDeleteBrews = true,
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true   // bypass email confirmation requirement for testing purposes
                    };

                    await userManager.CreateAsync(user, adminPassword); // Create the new admin user with the adminPassword
                    await userManager.AddToRoleAsync(user, "Admin");    // Add "Admin" role to the new user
                }

                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization(); // Allows use of identity authorisation capabilities

            app.MapStaticAssets();

            // Set default controller route to HomebrewItems controller and Index.cshtml page with no specified id
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=HomebrewItems}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}