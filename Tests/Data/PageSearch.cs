using System.Linq;
using System.Threading.Tasks;
using GithubAPIQuery;
using Newtonsoft.Json;

namespace Tests.Data
{
    public class PageSearch : IPageSearch
    {
        private readonly string _criteria;
        private readonly Repository[] _repositories;
        private readonly int _resultsPerPage;

        public PageSearch(string criteria, int resultsPerPage, Repository[] repositories)
        {
            _criteria = criteria;
            _resultsPerPage = resultsPerPage;
            _repositories = repositories;
        }

        public Task<string> GetPage(int page = 1)
        {
            if (ApiLimitExceeded(page)) return Task.Run(() => JsonConvert.SerializeObject(new ApiLimitExceeded()));
            var repositories = GetRepositories(page);
            var result = new RepositoryPage {TotalCount = _repositories.Length, Items = repositories};
            return Task.Run(() => JsonConvert.SerializeObject(result));
        }

        private Repository[] GetRepositories(int page)
        {
            return _repositories
                .Where(x => x.Name != null && x.Name.ToLowerInvariant().Contains(_criteria.ToLowerInvariant()))
                .Skip(_resultsPerPage*(page - 1))
                .Take(_resultsPerPage)
                .ToArray();
        }

        private bool ApiLimitExceeded(int page)
        {
            return _repositories
                .Skip(_resultsPerPage*(page - 1))
                .Take(_resultsPerPage)
                .OfType<ApiLimitExceeded>()
                .Any();
        }
    }
}