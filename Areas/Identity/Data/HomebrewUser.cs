using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

/// <summary>
/// A registered user of the application
/// </summary>
public class HomebrewUser : IdentityUser
{
    /// <summary>
    /// <c>DisplayName</c> is the user's desired name as defined upon registration; the Author field of HomebrewItem should be set to this
    /// This property is also referenced as 'DisplayNameInput' in UserAccountsContext.cs
    /// </summary>
    public string? DisplayName { get; set; }
    /// <summary>
    /// <c>CanMakeBrews</c>: whether or not the user can create new HomebrewItems
    /// </summary>
    [DefaultValue(true)]
    public bool CanMakeBrews { get; set; }
    /// <summary>
    /// <c>CanDeleteBrews</c>: whether or not the user can delete HomebrewItems that they have not created (i.e. are not in BrewsCreated)
    /// </summary>
    [DefaultValue(false)]
    public bool CanDeleteBrews { get; set; }
    /// <summary>
    /// <c>BrewsCreated</c> is a list of integer IDs which reference specific HomebrewItems. Initialised as empty upon user registration.
    /// </summary>
    public List<int>? BrewsCreated { get; set; }
}

