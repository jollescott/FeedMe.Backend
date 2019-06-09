using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ramsey.NET.Auto.Extensions
{
    public static class StringExts
    {
        private static readonly Regex Regex = new Regex("[^0-9a-zA-Z åäöèîéÅÄÖ]+");

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, string.Empty).Trim();
        }
    }
}
