using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Super_Simple_Homebrew_Hoster.Models
{
    public class HomebrewItemSystemViewModel
    {
        public List<HomebrewItem>? HomebrewItems { get; set; }
        public SelectList? Systems { get; set; }
        public string? ItemSystem { get; set; }
        public string? SearchString { get; set; }
    }
}
