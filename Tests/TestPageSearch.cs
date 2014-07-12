using System.Linq;
using System.Threading.Tasks;
using GithubAPIQuery;
using Newtonsoft.Json;

namespace Tests
{
    public class TestPageSearch : IPageSearch
    {
        public TestPageSearch(string criteria, int resultsPerPage, TestRepository[] repositories)
        {
            _criteria = criteria;
            _resultsPerPage = resultsPerPage;
            _repositories = repositories;
        }

        private readonly string _criteria;
        private readonly TestRepository[] _repositories;
        private readonly int _resultsPerPage;

        public Task<string> GetPage(int page = 1)
        {
            TestRepository[] repositories = _repositories
                .Where(x => x.Name != null && x.Name.ToLowerInvariant().Contains(_criteria.ToLowerInvariant()))
                .Skip(_resultsPerPage*(page - 1))
                .Take(_resultsPerPage)
                .ToArray();

            var result = new TestRepositoryPage {TotalCount = _repositories.Length, Items = repositories};
            return Task.Run(() => JsonConvert.SerializeObject(result));
        }
    }
}