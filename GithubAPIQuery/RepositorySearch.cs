using System.Net.Http;
using System.Threading.Tasks;

namespace GithubAPIQuery
{
    /// <summary>
    /// Searches the Github repository API for repositories matching a given criteria.
    /// </summary>
    public class RepositorySearch
    {
        

        private readonly string _urlFormat;

        /// <param name="criteria">The search criteria.</param>
        /// <param name="resultsPerPage">The number of results per page to be requested from the Github API.</param>
        public RepositorySearch(string criteria, int resultsPerPage = 10)
        {
            // E.g. https://api.github.com/search/repositories?q=raven&per_page=2&page=2
            _urlFormat = string.Format(@"https://api.github.com/search/repositories?q={0}&page={{0}}&per_page={1}", criteria, resultsPerPage);
        }

        /// <summary>
        /// Gets the JSON reponse for a given page.
        /// </summary>
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