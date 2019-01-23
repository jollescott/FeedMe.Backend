using HtmlAgilityPack;
using Ramsey.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ramsey.NET.Auto
{
    public interface IAutoConfig
    {
        //Provider Settings
        RamseyLocale ProviderLocale { get; }
        string ProviderId { get; }

        string ProviderLogo { get; }
        string ProviderName { get; }

        int RecipeCount { get; }
        int PageItemCount { get; }

        //Overrides

        /// <summary>
        /// Can be null
        /// </summary>
        Func<HtmlDocument, string> LoadImage { get; }

        /// <summary>
        /// Can be null
        /// </summary>
        Func<string, string> ParseId { get; }

        /// <summary>
        /// CANNOT be null
        /// </summary>
        Func<int, HtmlDocument, string> NextPage { get; }

        //Scraper Settings
        string RootPage { get; }
        string RecipeItemXPath { get; }

        //XPaths
        string ImageXPath { get; }
        string DirectionsXPath { get; }
        string IngredientsXPath { get; }
        string NameXPath { get; }
    }
}
