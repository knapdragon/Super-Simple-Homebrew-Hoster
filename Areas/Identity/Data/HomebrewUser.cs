using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

/// <summary>
/// A registered user of the application
/// </summary>
public class HomebrewUser : IdentityUser
{
    /// <summary>
    /// <c>CanMakeBrews</c>: whether or not the user can create new HomebrewItems
    /// </summary>
    [DefaultValue(true)]
    public bool CanMakeBrews { get; set; } = true;
    /// <summary>
    /// <c>CanDeleteBrews</c>: whether or not the user can delete HomebrewItems that they have not created (i.e. are not in BrewsCreated)
    /// </summary>
    [DefaultValue(false)]
    public bool CanDeleteBrews { get; set; } = false;
    /// <summary>
    /// <c>BrewsCreated</c> is a list of integer IDs which reference specific HomebrewItems. Initialised as empty upon user registration.
    /// </summary>
    public List<int>? BrewsCreated { get; set; }
}

