using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GithubAPIQuery;
using Newtonsoft.Json;
using Xunit;

namespace Tests
{
    public class TestSearchFactory : IPageSearchFactory
    {
        public IEnumerable<IPageSearch> GetSearches(int count, string criteria, int resultsPerPage)
        {
            if (count < 1) throw new ArgumentOutOfRangeException("count", "Must get at least one search.");
            return Enumerable.Range(1, count).Select(x => new TestPageSearch(criteria, resultsPerPage));
        }
    }

    public class TestPageSearch : IPageSearch
    {
        private string _criteria;
        private int _resultsPerPage;

        public TestPageSearch(string criteria, int resultsPerPage)
        {
            _criteria = criteria;
            _resultsPerPage = resultsPerPage;
        }

        public Task<string> GetPage(int page = 1)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestRepositoryPage
    {

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        public TestRepositoryPage()
        {
            Items=new TestRepository[0];
        }

        [JsonProperty(PropertyName = "items")]
        public TestRepository[] Items { get; set; }
    }
    
    public class TestRepository
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }

    public class UnitTests
    {
        [Fact]
        public void RecognisesRateLimitExceededMessage()
        {
            var rateLimit = File.ReadAllText("rate-limit-message.json");
            Assert.True(rateLimit.IsApiRateLimitWarning());

            var other = File.ReadAllText("result-content-with-two-repositories.json");
            Assert.False(other.IsApiRateLimitWarning());
        }

        [Fact]
        public void RepositoryDetailsAreParsedFromGithubJson()
        {
            var json = File.ReadAllText("result-content-with-two-repositories.json");
            var details = RepositoryDetails.FromJson(json).ToArray();

            Assert.Equal(2, details.Length);
            Assert.NotNull(details[0].Name);
            Assert.NotNull(details[0].Description);
        }

        [Fact]
        public void SearchPageCounterReturnsIncrementalPageNumbers()
        {
            var counter = new SearchPageCounter();
            foreach (var i in Enumerable.Range(1, 300))
            {
                Assert.Equal(i, counter.NextSearchPageNumber());
            }
        }
    }
}