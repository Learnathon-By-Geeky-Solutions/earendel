using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace TalentMesh.Framework.Infrastructure.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtensions
    {
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500); // Set a reasonable timeout

        public static string GetDescription(this Enum enumValue)
        {
            object[] attr = enumValue.GetType().GetField(enumValue.ToString())!
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attr.Length > 0)
                return ((DescriptionAttribute)attr[0]).Description;

            string result = enumValue.ToString();
            result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2", RegexOptions.None, RegexTimeout);
            result = Regex.Replace(result, "([A-Za-z])([0-9])", "$1 $2", RegexOptions.None, RegexTimeout);
            result = Regex.Replace(result, "([0-9])([A-Za-z])", "$1 $2", RegexOptions.None, RegexTimeout);
            result = Regex.Replace(result, "(?<!^)(?<! )([A-Z][a-z])", " $1", RegexOptions.None, RegexTimeout);

            return result;
        }

        public static ReadOnlyCollection<string> GetDescriptionList(this Enum enumValue)
        {
            string result = enumValue.GetDescription();
            return new ReadOnlyCollection<string>(result.Split(',').ToList());
        }
    }
}
