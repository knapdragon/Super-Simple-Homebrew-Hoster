using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Super_Simple_Homebrew_Hoster.Data;
using Super_Simple_Homebrew_Hoster.Models;
using System;
using System.Linq;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new Super_Simple_Homebrew_HosterContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<Super_Simple_Homebrew_HosterContext>>()))
        {
            // Look for any movies.
            if (context.HomebrewItem.Any())
            {
                return;   // DB has been seeded
            }
            context.HomebrewItem.AddRange(
                new HomebrewItem
                {
                    Title = "Circle of the Nomad",
                    ReleaseDate = DateTime.Parse("2024-10-28"),
                    Type = "Subclass",
                    System = "D&D 5E",
                }
            );
            context.SaveChanges();
        }
    }
}
