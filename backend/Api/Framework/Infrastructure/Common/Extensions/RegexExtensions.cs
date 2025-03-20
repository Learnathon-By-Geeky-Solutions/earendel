using System;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RegexExtensions
    {
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500); // Set a reasonable timeout
        private static readonly Regex Whitespace = new(@"\s+", RegexOptions.None, RegexTimeout);

        public static string ReplaceWhitespace(this string input, string replacement)
        {
            return Whitespace.Replace(input, replacement);
        }
    }
}
