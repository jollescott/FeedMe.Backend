using Newtonsoft.Json;
using Ramsey.NET.Ingredients.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ramsey.NET.Ingredients.Implementations
{
    public class BasicIngredientResolver : ABasicIngredientResolver
    {
        private IList<string> _regex;
        private IList<string> _removal;
        private IDictionary<string, IList<string>> _synonyms;

        public override Task<string> ApplyRegexesAsync(string ingredient)
        {
            var output = ingredient.ToLower();

            foreach(var pattern in _regex)
            {
                //Regex regex = new Regex("\\(.*?\\)");
                Regex regex = new Regex(pattern);
                output = regex.Replace(output, string.Empty);
            }

            return Task.FromResult(output);
        }

        public override void Init(IRamseyContext,IList<string> removal, IDictionary<string, IList<string>> synonyms)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (!File.Exists(Path.Join(path, "/Resources/regex.json")) || !File.Exists(Path.Join(path, "/Resources/removal.json")) || !File.Exists(Path.Join(path, "/Resources/synonyms.json")))
                throw new Exception("Missing resource files!");

            var regexJson = File.ReadAllText(Path.Join(path, "/Resources/regex.json"), System.Text.Encoding.GetEncoding(1252));
            //var removalJson = File.ReadAllText(Path.Join(path, "/Resources/removal.json"), System.Text.Encoding.GetEncoding(1252));
            //var synonymsJson = File.ReadAllText(Path.Join(path, "/Resources/synonyms.json"), System.Text.Encoding.GetEncoding(1252));

            try
            {
                _regex = JsonConvert.DeserializeObject<IList<string>>(regexJson);
                //_removal = JsonConvert.DeserializeObject<IList<string>>(removalJson);
                //_synonyms = JsonConvert.DeserializeObject<IDictionary<string, IList<string>>>(synonymsJson);

                _removal = removal;
                _synonyms = synonyms;
            }
            catch (Newtonsoft.Json.JsonSerializationException ex)
            {
                throw new Exception("Resources are malformed! " + ex.Message);
            }
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

        public override void RemoveIllegals(ref string name)
        {
            foreach (var word in _removal)
            {
                if (name.Contains(word) && word != string.Empty)
                    name = name.Substring(0, name.IndexOf(word)).Trim();
            }
        }

        public override async Task<string> ResolveIngredientAsync(string ingredient)
        {
            var output = ingredient;

            output = await ApplyRegexesAsync(output);
            RemoveIllegals(ref ingredient);
            output = await LinkSynonymsAsync(output);

            return output;
        }
    }
}
