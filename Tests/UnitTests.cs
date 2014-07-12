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
        public void RecognisesRateLimitExceededMessage()
        {
            string rateLimit = File.ReadAllText("rate-limit-message.json");
            Assert.True(rateLimit.IsApiRateLimitWarning());

            string other = File.ReadAllText("result-content-with-two-repositories.json");
            Assert.False(other.IsApiRateLimitWarning());
        }

        [Fact]
        public void RepositoryDetailsAreParsedFromGithubJson()
        {
            string json = File.ReadAllText("result-content-with-two-repositories.json");
            RepositoryDetails[] details = RepositoryDetails.FromJson(json).ToArray();

            Assert.Equal(2, details.Length);
            Assert.NotNull(details[0].Name);
            Assert.NotNull(details[0].Description);
        }

        [Fact]
        public void SearchPageCounterReturnsIncrementalPageNumbers()
        {
            var counter = new SearchPageCounter();
            foreach (int i in Enumerable.Range(1, 300))
            {
                Assert.Equal(i, counter.NextSearchPageNumber());
            }
        }

        [Fact]
        public void TestRepositoryPagesSerialiseAsExpected()
        {
            var builder = new TestRepositoryBuilder();
            TestRepository[] repositories = builder.Build();

            string json = JsonConvert.SerializeObject(repositories);
            Console.WriteLine(json);

            var deserialised = JsonConvert.DeserializeObject<TestRepositoryPage>(json);
            Assert.Equal(repositories.Length, deserialised.Items.Length);

            TestRepositoryPage empty = builder.Empty;
            Assert.Equal(builder.TotalCount, empty.TotalCount);
            Assert.Equal(0, empty.Items.Length);
        }

        [Fact]
        public void TestSearchFactoryReturnsWorkingFakeSearches()
        {
            var factory = new TestSearchFactory();
            IPageSearch search = factory.GetSearches(1, "raven", 100).Single();

            string json = search.GetPage().Result;
            RepositoryDetails[] details = RepositoryDetails.FromJson(json).ToArray();

            Assert.Equal(100, details.Length);
        }
    }
}