using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Microsoft.AspNetCore.Html;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Super_Simple_Homebrew_Hoster.Models
{
    /// <summary>
    /// Each item in the index is a 'Homebrew', a custom content creation made by a given person for the purposes of implementing it into their own games.
    /// </summary>
    public class HomebrewItem
    {
        public int Id { get; set; }

        /// <value>
        /// <c>Title</c>: the author's title for the item
        /// </value>
        public string? Title { get; set; }

        /// <value>
        /// <c>Type</c>: the user-specified type of item, e.g. class, monster, equipment, collection of things, etc
        /// </value>
        public string? Type { get; set; }

        /// <value>
        /// <c>Version</c>: the user-specified version
        /// </value>
        public string? Version { get; set; }

        /// <value>
        /// <c>Source</c>: the user-specified source if they describe one, e.g. Xanathar's Guide to Everything
        /// </value>
        public string? Source { get; set; }

        /// <value>
        /// <c>System</c>: the system the item was designed for, e.g. D&D 5e, PF2e
        /// </value>
        public string? System { get; set; }

        /// <value>
        /// <c>Author</c>: the user's DisplayName as supplied upon registration.
        /// </value>
        public string? Author { get; set; } 

        /// <value>
        /// <c>ReleaseDate</c>: the user-specified initial release date of the item
        /// </value>
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; } 

        /// <value>
        /// <c>Uri</c>: the Uniform Resource Identifier (aka URL) for the item, if they have a Google Doc or Homebrewery page for it
        /// </value>
        public Uri? Link { get; set; } 

        /// <value>
        /// <c>Content</c>: the actual content of the homebrew. This is seen as HTML in the TinyMCE editor.
        /// </value>
        public string? Content { get; set; } 
    }
}
