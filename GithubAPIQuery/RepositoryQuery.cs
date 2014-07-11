using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GithubAPIQuery
{
    public class RepositoryQuery
    {
        private readonly string _url;

        public RepositoryQuery(string criteria, int page = 1, int resultsPerPage = 10)
        {
            // E.g. https://api.github.com/search/repositories?q=raven&per_page=2&page=2
            _url = string.Format(@"https://api.github.com/search/repositories?q={0}&page={1}&per_page={2}", criteria, page, resultsPerPage);
        }

        public async Task<IEnumerable<RepositoryDetails>> GetDetails()
        {
            using (var client = new HttpClient())
            {
                // Github requires the user agent header to be set.  See here: https://developer.github.com/v3/#user-agent-required
                client.DefaultRequestHeaders.Add("User-Agent", "Seans-Code");
                var task = await client.GetAsync(_url);
                var json = await task.Content.ReadAsStringAsync();
                return RepositoryDetails.FromJson(json);
            }
        }
    }
}