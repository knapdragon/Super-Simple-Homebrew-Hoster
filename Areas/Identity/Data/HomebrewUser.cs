using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Super_Simple_Homebrew_Hoster.Models;

namespace Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

// TODO: Add DisplayName property for username customisation
/// <summary>
/// A registered user of the application
/// </summary>
public class HomebrewUser : IdentityUser
{
    /// <summary>
    /// <c>DisplayName</c> is the user's desired name; the Author field of HomebrewItem should be set to this
    /// This property is also referenced as 'UsernameInput' in UserAccountsContext.cs
    /// </summary>
    public string? DisplayName { get; set; }
    /// <summary>
    /// <c>CanMakeBrews</c>: a property referring to whether or not the user can create new HomebrewItems
    /// </summary>
    public bool CanMakeBrews { get; set; }
    /// <summary>
    /// <c>CanDeleteBrews</c>: a property referring to whether or not the user can delete HomebrewItems that they have not created (i.e. are not in BrewsCreated)
    /// </summary>
    public bool CanDeleteBrews { get; set; }
    /// <summary>
    /// <c>BrewsCreated</c> is a list of integer IDs which reference specific HomebrewItems
    /// </summary>
    public List<int>? BrewsCreated { get; set; }
}

