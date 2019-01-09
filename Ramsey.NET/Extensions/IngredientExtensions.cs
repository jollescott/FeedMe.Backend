using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ramsey.NET.Extensions
{
    public static class IngredientExtensions
    {
        private static readonly IIngredientFormatPipeline[] _formatPipeline = new IIngredientFormatPipeline[]{
            new ColonFormatPipeline(),
            new TexFormatPipeline(),
            new HyphenFormatPipeline(),
            new ParenthesesFormatPipeline(),
            new DictionaryFormatPipeline(),
            new CommaFormatPipeline(),
            new EllerFormatPipeline(),
            new TillFormatPipeline(),
            new TrimFormatPipeline(),
            new AltFormatPipeline(),
            new EfterFormatPipeline()
        };

        public static string FormatIngredientName(this string ingredient)
        {
            string output = ingredient;

            foreach(var pipe in _formatPipeline)
            {
                output = pipe.Format(output);
            }

            return output;
        }
    }

    public interface IIngredientFormatPipeline
    {
        string Format(string input);
    }

    public class TrimFormatPipeline : IIngredientFormatPipeline
    {
        public string Format(string input)
        {
            return input.Trim();
        }
    }

    public class ColonFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => ":";
    }

    public class EllerFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => "eller";
    }

    public class TillFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => "till";
    }

    public class TexFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => "t ex";
    }

    public class HyphenFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => "-";
    }

    public class CommaFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => ",";
    }

    public class AltFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => "alt";
    }

    public class EfterFormatPipeline : WordRemovalFormatPipeline
    {
        public override string Word => "efter";
    }

    public class WordRemovalFormatPipeline : IIngredientFormatPipeline
    {
        public virtual string Word => string.Empty;

        public string Format(string input)
        {
            if (input.Contains(Word) && Word != string.Empty)
                return input.Substring(0, input.IndexOf(Word)).Trim();
            else
                return input;
        }
    }

    public class ParenthesesFormatPipeline : IIngredientFormatPipeline
    {
        public string Format(string input)
        {
            Regex regex = new Regex("\\(.*?\\)");
            return regex.Replace(input, string.Empty);
        }
    }

    public class DictionaryFormatPipeline : IIngredientFormatPipeline
    {
        private static readonly string[] _ignored = new string[]
        {
            "så det räcker",
            "så det täcker frukten",
            "med äkta vanilj",
            "i vatten",
            "bit färsk"
        };

        public string Format(string input)
        {
            string output = input;

            foreach(var word in _ignored)
            {
                output = output.Replace(word, string.Empty);
            }

            return output;
        }
    }
}
