using System.Linq;
using System.Threading.Tasks;
using GithubAPIQuery;
using Newtonsoft.Json;

namespace Tests.Data
{
    public class TestPageSearch : IPageSearch
    {
        private readonly string _criteria;
        private readonly TestRepository[] _repositories;
        private readonly int _resultsPerPage;

        public TestPageSearch(string criteria, int resultsPerPage, TestRepository[] repositories)
        {
            _criteria = criteria;
            _resultsPerPage = resultsPerPage;
            _repositories = repositories;
        }

        public Task<string> GetPage(int page = 1)
        {
            if (ApiLimitExceeded(page)) return Task.Run(() => JsonConvert.SerializeObject(new ApiLimitExceeded()));
            var repositories = GetRepositories(page);
            var result = new TestRepositoryPage {TotalCount = _repositories.Length, Items = repositories};
            return Task.Run(() => JsonConvert.SerializeObject(result));
        }

        private TestRepository[] GetRepositories(int page)
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