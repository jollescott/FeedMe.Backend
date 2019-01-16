using Newtonsoft.Json;
using Ramsey.NET.Ingredients.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Implementations
{
    public class BasicIngredientResolver : ABasicIngredientResolver
    {
        private readonly IList<string> _regex;
        private readonly IList<string> _removal;
        private readonly IDictionary<string, IList<string>> _synonyms;

        public BasicIngredientResolver()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!File.Exists(Path.Join(path, "/Resources/regex.json")) || !File.Exists(Path.Join(path,"/Resources/removal.json")) || !File.Exists(Path.Join(path,"/Resources/synonyms.json")))
                throw new Exception("Missing resource files!");

            var regexJson = File.ReadAllText(Path.Join(path, "/Resources/regex.json"));
            var removalJson = File.ReadAllText(Path.Join(path, "/Resources/removal.json"));
            var synonymsJson = File.ReadAllText(Path.Join(path, "/Resources/synonyms.json"));

            try
            {
                _regex = JsonConvert.DeserializeObject<IList<string>>(regexJson);
                _removal = JsonConvert.DeserializeObject<IList<string>>(removalJson);
                _synonyms = JsonConvert.DeserializeObject<IDictionary<string, IList<string>>>(synonymsJson);
            }
            catch(Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new Exception("Resources are malformed! " + ex.Message);
            }
        }

        public override Task<string> ApplyRegexesAsync(string ingredient)
        {
            var output = ingredient;

            foreach(var pattern in _regex)
            {
                //Regex regex = new Regex("\\(.*?\\)");
                Regex regex = new Regex(pattern);
                output = regex.Replace(output, string.Empty);
            }

            return Task.FromResult(output);
        }

        public override Task<string> LinkSynonymsAsync(string ingredient)
        {
            foreach(var key in _synonyms.Keys)
            {
                var synonyms = _synonyms[key];

                if (synonyms.Any(x => x == ingredient))
                    return Task.FromResult(key);
            }

            return Task.FromResult(ingredient);
        }

        public override Task<string> RemoveIllegalsAsync(string ingredient)
        {
            var output = ingredient;

            foreach(var word in _removal)
            {
                if (output.Contains(word) && word != string.Empty)
                    output = output.Substring(0, output.IndexOf(word)).Trim();
            }

            return Task.FromResult(output);
        }

        public override async Task<string> ResolveIngredientAsync(string ingredient)
        {
            var output = ingredient;

            output = await ApplyRegexesAsync(output);
            output = await RemoveIllegalsAsync(output);
            output = await LinkSynonymsAsync(output);

            return output;
        }
    }
}
