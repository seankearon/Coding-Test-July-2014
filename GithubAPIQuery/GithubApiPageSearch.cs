using System.Net.Http;
using System.Threading.Tasks;

namespace GithubAPIQuery
{
    /// <summary>
    ///     Searches the Github API for repositories matching a given criteria, retreiving a single page of results.
    /// </summary>
    public class GithubApiPageSearch : IPageSearch
    {
        /// <summary>
        ///     E.g. https://api.github.com/search/repositories?q=raven&per_page=2&page=2
        /// </summary>
        private const string UrlFormatStub = @"https://api.github.com/search/repositories?q={0}&page={{0}}&per_page={1}";

        private readonly string _urlFormat;

        /// <param name="criteria">The search criteria.</param>
        /// <param name="resultsPerPage">The number of results per page to be requested from the Github API.</param>
        public GithubApiPageSearch(string criteria, int resultsPerPage = 100)
        {
            _urlFormat = string.Format(UrlFormatStub, criteria, resultsPerPage);
        }

        public async Task<string> GetPage(int page = 1)
        {
            var url = string.Format(_urlFormat, page);
            using (var client = new HttpClient())
            {
                // Github requires the user agent header to be set.  See here: https://developer.github.com/v3/#user-agent-required
                client.DefaultRequestHeaders.Add("User-Agent", "Seans-Code");
                var task = await client.GetAsync(url);
                return await task.Content.ReadAsStringAsync();
            }
        }
    }
}