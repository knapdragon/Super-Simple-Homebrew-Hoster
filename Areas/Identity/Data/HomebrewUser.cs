using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Super_Simple_Homebrew_Hoster.Models;

namespace Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

// Add profile data for application users by adding properties to the HomebrewUser class
// TODO: Add DisplayName property for username customisation
public class HomebrewUser : IdentityUser
{
    public bool CanMakeBrews { get; set; }
    public bool CanDeleteBrews { get; set; }
    public List<int>? BrewsCreated { get; set; }
}

