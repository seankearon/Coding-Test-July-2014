using System;
using System.Collections.Generic;
using System.Linq;

namespace GithubAPIQuery
{
    public class PageSearchFactory : IPageSearchFactory
    {
        public IEnumerable<IPageSearch> GetSearches(int count, string criteria, int resultsPerPage)
        {
            if (count < 1) throw new ArgumentOutOfRangeException("count", "Must create one or more searches.");
            return Enumerable.Range(1, count).Select(x => new GithubApiPageSearch(criteria, resultsPerPage));
        }
    }
}