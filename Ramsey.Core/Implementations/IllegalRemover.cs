using Microsoft.EntityFrameworkCore.Internal;
using Ramsey.Core.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;

namespace Ramsey.Core.Implementations
{
    public class IllegalRemover : IWordRemover
    {
        private readonly IRamseyContext _ramseyContext;

        private readonly string[] _breakWords = {
            "och",
            "eller",
            "som",
            "till",
            "gärna"
        };

        public IllegalRemover(IRamseyContext ramseyContext)
        {
            _ramseyContext = ramseyContext;
        }

        public string RemoveIllegals(string name)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var output = name;

            var words = output.Split(' ');

            foreach (var word in words)
            {
                if (_ramseyContext.BadWords.Any(x => x.Word == word))
                {
                    output = output.Replace(word, string.Empty);
                }

                if (_breakWords.Any(x => x == word))
                {
                    //Check if it STILL contains
                    if (output.Contains(word))
                        output = output.Substring(0, output.IndexOf(word, StringComparison.Ordinal));
                }
            }

            var synonym = _ramseyContext.IngredientSynonyms.FirstOrDefault(x => x.Wrong == output);

            if (synonym != null)
                output = synonym.Correct;

            stopWatch.Stop();
            return output;
        }
    }
}
