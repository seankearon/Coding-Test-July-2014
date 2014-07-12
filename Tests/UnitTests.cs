using System.IO;
using System.Linq;
using GithubAPIQuery;
using Xunit;

namespace Tests
{
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