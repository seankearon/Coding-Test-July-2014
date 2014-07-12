using System.Collections.Generic;

namespace GithubAPIQuery
{
    public interface IPageSearchFactory
    {
        IEnumerable<IPageSearch> GetSearches(int count, string criteria, int resultsPerPage);
    }
}