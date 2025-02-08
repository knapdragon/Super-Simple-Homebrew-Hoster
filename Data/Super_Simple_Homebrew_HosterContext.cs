using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Super_Simple_Homebrew_Hoster.Models;

namespace Super_Simple_Homebrew_Hoster.Data
{
    public class Super_Simple_Homebrew_HosterContext : DbContext
    {
        public Super_Simple_Homebrew_HosterContext (DbContextOptions<Super_Simple_Homebrew_HosterContext> options)
            : base(options)
        {
        }

        public DbSet<Super_Simple_Homebrew_Hoster.Models.HomebrewItem> HomebrewItem { get; set; } = default!;
    }
}
