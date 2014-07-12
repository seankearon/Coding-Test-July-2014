using System;
using System.Linq;
using GithubAPIQuery;
using Xunit;

namespace Tests
{
    public class Integration
    {
        [Fact]
        public void RepositorySearchReturnsSpecifiedNumberOfResults()
        {
            var query = new GithubApiPageSearch("raven", 4);
            string json = query.GetPage().Result;

            RepositoryDetails[] details = RepositoryDetails.FromJson(json).ToArray();
            Assert.Equal(4, details.Length);
        }

        [Fact]
        public void RepositorySearcherReturnsResults()
        {
            var searcher = new RepositorySearch();
            RepositoryDetails[] results = searcher.RunSearch("raven");

            Console.WriteLine("Found {0} repositories.", results.Length);
            foreach (RepositoryDetails repository in results)
            {
                Console.WriteLine("    {0} - {1}", repository.Name, repository.Description);
            }

            string metadata = new GithubApiPageSearch("raven", 1).GetPage().Result;
            int expectedResultCount = metadata.GetApiTotalCount();
            Assert.Equal(expectedResultCount, results.Length);
        }
    }
}