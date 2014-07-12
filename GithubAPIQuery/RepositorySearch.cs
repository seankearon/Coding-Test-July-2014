using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubAPIQuery
{
    /// <remarks>
    ///     The brief is to optimise for the time spent on the network I/O.
    ///     <see cref="RepositorySearch" /> achieves this by allowing clients control over
    ///     the number of concurrent connections and the results per page.
    /// </remarks>
    public class RepositorySearch
    {
        private readonly IPageSearchFactory _factory;
        private readonly ConcurrentBag<RepositoryDetails> _results = new ConcurrentBag<RepositoryDetails>();

        /// <remarks>This will only ever set to a single value. Hence, no locking required.</remarks>
        private bool _keepSearching = true;

        private SearchPageCounter _pageCounter;
        private bool _searching = false;

        public RepositorySearch(IPageSearchFactory factory)
        {
            if (factory == null) throw new ArgumentException("factory");
            _factory = factory;
        }

        public RepositorySearch() : this(new PageSearchFactory())
        {
        }

        /// <remarks>
        ///     The approach here is use N independent tasks that each retreive a separate page.
        ///     After the page is retreived the task will get other pages until the whole job is complete.
        /// </remarks>
        public RepositoryDetails[] RunSearch(string criteria, int maxConcurrentQueries = 10, int resultsPerPage = 100)
        {
            if (string.IsNullOrWhiteSpace(criteria)) throw new ArgumentException("Criteria required.", "criteria");
            if (maxConcurrentQueries < 1) throw new ArgumentException("Must have at least one query.", "maxConcurrentQueries");
            if (_searching) throw new ApplicationException("Search already in progress.");
            _searching = true;

            _pageCounter = new SearchPageCounter();
            var searches = GetRepositorySearches(criteria, maxConcurrentQueries, resultsPerPage);

            var tasks = searches.Select(search => Task.Factory.StartNew(() =>
            {
                while (_keepSearching)
                {
                    var pageNumber = _pageCounter.NextSearchPageNumber();
                    var json = search.GetPage(pageNumber).Result;
                    if (json.IsApiRateLimitWarning()) throw new ApplicationException("API rate limit exceeded.");
                    HarvestDetails(json);

                    if (!json.HasRepositories())
                    {
                        // A page has no results so assume we're complete. 
                        _keepSearching = false;
                    }
                }
            })).ToArray();

            Task.WaitAll(tasks);
            _searching = false;
            return _results.OrderBy(x => x.Name).ToArray();
        }

        private IEnumerable<GithubApiPageSearch> GetRepositorySearches(string criteria, int maxConcurrentQueries, int resultsPerPage)
        {
            return Enumerable
                .Range(1, maxConcurrentQueries)
                .Select(i => new GithubApiPageSearch(criteria, resultsPerPage));
        }

        /// <summary>
        ///     Gets and stores the repository details from the response JSON.
        /// </summary>
        /// <param name="json">The response JSON from the Github API.</param>
        private void HarvestDetails(string json)
        {
            var details = RepositoryDetails.FromJson(json);
            foreach (var detail in details)
            {
                _results.Add(detail);
            }
        }
    }
}