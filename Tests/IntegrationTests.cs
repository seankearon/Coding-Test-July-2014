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
            var query = new GithubPageSearch("raven", 4);
            var json = query.GetPage().Result;

            var details = RepositoryDetails.FromJson(json).ToArray();
            Assert.Equal(4, details.Length);
        }

        [Fact]
        public void RepositorySearcherReturnsResults()
        {
            var searcher = new RepositorySearch();
            var results = searcher.RunSearch("raven");

            Console.WriteLine("Found {0} repositories.", results.Length);
            foreach (var repository in results)
            {
                Console.WriteLine("    {0} - {1}", repository.Name, repository.Description);
            }

            var metadata = new GithubPageSearch("raven", 1).GetPage().Result;
            var expectedResultCount = metadata.GetApiTotalCount();
            Assert.Equal(expectedResultCount, results.Length);
        }
    }
}