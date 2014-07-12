using System;
using System.IO;
using System.Linq;
using GithubAPIQuery;
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
            var results = searcher.RunSearch(TestRepositoryBuilder.DefaultCriteria);

            Assert.NotEmpty(results);
            Assert.Equal(repositories.Length, results.Length);
        }

        [Fact]
        public void RepositorySearcherCanReturnEmptyResults()
        {
            var repositories = new TestRepositoryBuilder().Build();
            var factory = new TestSearchFactory(repositories);

            var searcher = new RepositorySearch(factory);
            var results = searcher.RunSearch("this-will-not-be-found");

            Assert.Empty(results);
        }

        [Fact]
        public void RepositorySearcherFailsWhenRateLimitExceeded()
        {
            var repositories = new TestRepositoryBuilder()
                .WithTotalCount(10)
                .WithApiLimit(9)
                .Build();
            var factory = new TestSearchFactory(repositories);

            var searcher = new RepositorySearch(factory);

            // Expecting an ApplicationException aggregated by the TPL.
            var aggregate = Assert.Throws<AggregateException>(() => searcher.RunSearch(TestRepositoryBuilder.DefaultCriteria));
            Assert.IsType<ApplicationException>(aggregate.InnerException);
        }

        [Fact]
        public void ApiRateLimitExceededMessageRecognised()
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