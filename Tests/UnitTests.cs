using System;
using System.IO;
using System.Linq;
using GithubAPIQuery;
using Newtonsoft.Json;
using Tests.Data;
using Xunit;

namespace Tests
{
    public class UnitTests
    {
        [Fact]
        public void RepositorySearcherReturnsExpectedResults()
        {
            var repositories = new TestRepositoryBuilder().Build();
            var factory = new TestSearchFactory(repositories);
            
            var searcher = new RepositorySearch(factory);
            var results = searcher.RunSearch("raven");

            Assert.NotEmpty(results);
            Assert.Equal(repositories.Length, results.Length);
        }

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

        [Fact]
        public void TestRepositoryPagesSerialiseAsExpected()
        {
            var builder = new TestRepositoryBuilder();
            var repositories = builder.Build();

            var json = JsonConvert.SerializeObject(repositories);
            Console.WriteLine(json);

            var deserialised = JsonConvert.DeserializeObject<TestRepository[]>(json);
            Assert.Equal(repositories.Length, deserialised.Length);

            var empty = builder.Empty;
            Assert.Equal(builder.TotalCount, empty.TotalCount);
            Assert.Equal(0, empty.Items.Length);
        }

        [Fact]
        public void TestSearchFactoryReturnsWorkingFakeSearches()
        {
            var factory = new TestSearchFactory(new TestRepositoryBuilder().Build());
            var search = factory.GetSearches(1, "raven", 100).Single();

            var json = search.GetPage().Result;
            var details = RepositoryDetails.FromJson(json).ToArray();

            Assert.Equal(100, details.Length);
        }
    }
}