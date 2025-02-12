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
            builder.Services.AddDbContext<Super_Simple_Homebrew_HosterContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Super_Simple_Homebrew_HosterContext") ?? throw new InvalidOperationException("Connection string 'Super_Simple_Homebrew_HosterContext' not found.")));

            builder.Services.AddDbContext<UserAccountsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Super_Simple_Homebrew_HosterContext") ?? throw new InvalidOperationException("Connection string 'Super_Simple_Homebrew_HosterContext' not found.")));
            builder.Services.AddDefaultIdentity<HomebrewUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserAccountsContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "User", "Guest" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                SeedData.Initialize(services);
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                string adminEmail = "admin@admin.com";
                string adminPassword = "Test1234,";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = adminEmail;
                    user.Email = adminEmail;
                    user.EmailConfirmed = true;
                    
                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, "Admin");
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

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=HomebrewItems}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}