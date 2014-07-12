using Newtonsoft.Json.Linq;

namespace GithubAPIQuery
{
    public static class StringExtensions
    {
        /// <summary>
        ///     The API returns the total number of repositories for the search term.  Example warning response in
        ///     Tests\result-content-with-two-repositories.json.
        /// </summary>
        public static int GetApiTotalCount(this string json)
        {
            return JObject.Parse(json)["total_count"].Value<int>();
        }

        /// <summary>
        ///     Determines whether the search result JSON contains any repositories.
        /// </summary>
        public static bool HasRepositories(this string json)
        {
            var items = JObject.Parse(json)["items"];
            return items != null && items.HasValues;
        }

        /// <summary>
        ///     Github limits the API query rate.  Example warning response in Tests\rate-limit-message.json.
        /// </summary>
        public static bool IsApiRateLimitWarning(this string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return false;

            var message = JObject.Parse(json)["message"];
            if (message == null) return false;
            var value = message.Value<string>();
            if (string.IsNullOrEmpty(value)) return false;
            return value.ToLowerInvariant().StartsWith("api rate limit exceeded");
        }
    }
}