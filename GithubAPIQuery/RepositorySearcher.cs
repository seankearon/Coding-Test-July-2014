using System;
using System.Linq;

namespace GithubAPIQuery
{
    public class RepositorySearcher
    {
        private readonly RepositoryQuery[] _queries;
        private readonly SearchPageCounter _pageCounter;
        
        /// <remarks>
        /// This value is only ever changed to true. Hence, locking is not required.
        /// </remarks>
        private bool searchComplete = false;

        public RepositorySearcher(string criteria, int maxConcurrentQueries = 10, int resultsPerPage = 10)
        {
            if (string.IsNullOrWhiteSpace(criteria)) throw new ArgumentException("Criteria required.", "criteria");
            if (maxConcurrentQueries < 1) throw new ArgumentException("Must have at least one query.", "maxConcurrentQueries");

            _queries = Enumerable.Range(1, maxConcurrentQueries).Select(i => new RepositoryQuery(criteria, i, resultsPerPage)).ToArray();
            _pageCounter = new SearchPageCounter();
        }

        public RepositoryDetails[] RunQuery()
        {
            return new RepositoryDetails[0];
        }
    }
}