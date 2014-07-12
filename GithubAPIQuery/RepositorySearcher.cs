using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace GithubAPIQuery
{
    /// <remarks>
    ///     The brief is to optimise for the time spent on the network I/O.
    ///     <see cref="RepositorySearcher" /> achieves this by allowing clients control over
    ///     the number of concurrent connections and the results per page.
    /// </remarks>
    public class RepositorySearcher
    {
        public RepositorySearcher(string criteria, int maxConcurrentQueries = 10, int resultsPerPage = 100)
        {
            if (string.IsNullOrWhiteSpace(criteria)) throw new ArgumentException("Criteria required.", "criteria");
            if (maxConcurrentQueries < 1) throw new ArgumentException("Must have at least one query.", "maxConcurrentQueries");

            _searches = Enumerable
                .Range(1, maxConcurrentQueries)
                .Select(i => new RepositorySearch(criteria, resultsPerPage))
                .ToArray();
            _pageCounter = new SearchPageCounter();
        }

        /// <remarks>This is only ever set to false. Hence, locking is not required.</remarks>
        private bool _keepSearching = true;

        private readonly SearchPageCounter _pageCounter;
        private readonly ConcurrentBag<RepositoryDetails> _results = new ConcurrentBag<RepositoryDetails>();
        private readonly RepositorySearch[] _searches;

        /// <remarks>
        ///     The approach here is use N independent tasks that each retreive a separate page.
        ///     After the page is retreived the task will get other pages until the whole job is complete.
        /// </remarks>
        public RepositoryDetails[] RunSearch()
        {
            var searches = _searches.Select(search => Task.Factory.StartNew(() =>
            {
                while (_keepSearching)
                {
                    var pageNumber = _pageCounter.NextSearchPageNumber();
                    var json = search.GetPage(pageNumber).Result;
                    if (json.IsApiRateLimitWarning()) throw new ApplicationException("API rate limit exceeded.");
                    var detailsFound = HarvestRespostoryDetails(json);
                    if (!detailsFound)
                    {
                        // We assume we're complete a requested page has no results. 
                        // In which case the response will be something like this: {"total_count":801,"incomplete_results":false,"items":[]}
                        _keepSearching = false;
                    }
                }
            })).ToArray();

            Task.WaitAll(searches);
            return _results.OrderBy(x => x.Name).ToArray();
        }

        /// <summary>
        ///     Gets and stores the repository details from the response JSON.
        /// </summary>
        /// <param name="json">The response JSON from the Github API.</param>
        /// <returns>True if any repository details were found in <paramref name="json" />.</returns>
        private bool HarvestRespostoryDetails(string json)
        {
            var details = RepositoryDetails.FromJson(json);
            foreach (var detail in details)
            {
                _results.Add(detail);
            }
            return details.Any();
        }
    }
}