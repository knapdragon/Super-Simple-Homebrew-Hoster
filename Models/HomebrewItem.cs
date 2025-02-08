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
    public class HomebrewItem
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Type { get; set; }

        public string? Version { get; set; }

        public string? Source { get; set; }

        public string? System { get; set; }

        public string? Author { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public Uri? Link { get; set; }

        public string? Content { get; set; }
    }
}
