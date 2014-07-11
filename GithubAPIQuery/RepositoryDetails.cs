using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace GithubAPIQuery
{
    public class RepositoryDetails
    {
        public static IEnumerable<RepositoryDetails> FromJson(string json)
        {
            var o = JObject.Parse(json);
            return o["items"].Select(item => new RepositoryDetails { Name = Extensions.Value<string>(item["name"]), Description = Extensions.Value<string>(item["description"]) });
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}