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
            var query = new RepositorySearch("raven", 4);
            var json = query.GetPage().Result;

            var details = RepositoryDetails.FromJson(json).ToArray();
            Assert.Equal(4, details.Length);
        }

        [Fact]
        public void RepositorySearcherReturnsResults()
        {
            var searcher = new RepositorySearcher("raven");
            var results = searcher.RunSearch();

            Console.WriteLine("Found {0} repositories.", results.Length);
            foreach (var repository in results)
            {
                Console.WriteLine("    {0} - {1}", repository.Name, repository.Description);
            }

            var metadata = new RepositorySearch("raven", 1).GetPage().Result;
            var expectedResultCount = metadata.GetApiTotalCount();
            Assert.Equal(expectedResultCount, results.Length);
        }
    }
}