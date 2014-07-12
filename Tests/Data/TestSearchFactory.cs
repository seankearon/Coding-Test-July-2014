using System;
using System.Collections.Generic;
using System.Linq;
using GithubAPIQuery;

namespace Tests.Data
{
    public class TestSearchFactory : IPageSearchFactory
    {
        private readonly TestRepository[] _repositories;

        public TestSearchFactory(TestRepository[] repositories)
        {
            _repositories = repositories;
        }

        public IEnumerable<IPageSearch> GetSearches(int count, string criteria, int resultsPerPage)
        {
            if (count < 1) throw new ArgumentOutOfRangeException("count", "Must get at least one search.");
            return Enumerable.Range(1, count).Select(x => new TestPageSearch(criteria, resultsPerPage, _repositories));
        }
    }
}