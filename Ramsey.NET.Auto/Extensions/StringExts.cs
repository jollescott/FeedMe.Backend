﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ramsey.NET.Auto.Extensions
{
    public static class StringExts
    {
        private static readonly Regex regex = new Regex("[^0-9a-zA-Z åäöèî]+");

        public static string RemoveSpecialCharacters(this string str)
        {
            return regex.Replace(str, string.Empty);
        }
    }
}