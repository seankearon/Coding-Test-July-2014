using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace GithubAPIQuery
{
    public class RepositoryDetails
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public static IEnumerable<RepositoryDetails> FromJson(string json)
        {
            var items = JObject.Parse(json)["items"];
            return items != null
                ? items.Select(item => new RepositoryDetails {Name = item["name"].Value<string>(), Description = item["description"].Value<string>()})
                : Enumerable.Empty<RepositoryDetails>();
        }
    }
}