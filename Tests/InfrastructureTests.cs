using System;
using System.Linq;
using GithubAPIQuery;
using Newtonsoft.Json;
using Tests.Data;
using Xunit;

namespace Tests
{
    /// <summary>
    /// These tests assert on the infrastructure used for the testing of the "real" code.
    /// </summary>
    public class InfrastructureTests
    {
        [Fact]
        public void TestRepositoryPagesSerialiseAsExpected()
        {
            var builder = new RepositoryBuilder();
            var repositories = builder.Build();

            var json = JsonConvert.SerializeObject(repositories);
            Console.WriteLine(json);

            var deserialised = JsonConvert.DeserializeObject<Repository[]>(json);
            Assert.Equal(repositories.Length, deserialised.Length);

            var empty = builder.Empty;
            Assert.Equal(builder.TotalCount, empty.TotalCount);
            Assert.Equal(0, empty.Items.Length);
        }

        [Fact]
        public void ApiLimitExceededPretendsToBeAnErrorMessage()
        {
            var exceeded = new ApiLimitExceeded { Name = "qwe", Description = "asd" };
            var json = JsonConvert.SerializeObject(exceeded);
            Assert.True(json.IsApiRateLimitWarning());
        }

        [Fact]
        public void TestSearchFactoryReturnsWorkingFakeSearches()
        {
            var factory = new SearchFactory(new RepositoryBuilder().Build());
            var search = factory.GetSearches(1, RepositoryBuilder.DefaultCriteria, 100).Single();

            var json = search.GetPage().Result;
            var details = RepositoryDetails.FromJson(json).ToArray();

            Assert.Equal(100, details.Length);
        }

        [Fact]
        public void TestSearchFactoryCanModelsTheApiLimit()
        {
            var builder = new RepositoryBuilder().WithTotalCount(10).WithApiLimit(9);
            var factory = new SearchFactory(builder.Build());
            var search = factory.GetSearches(1, RepositoryBuilder.DefaultCriteria, 100).Single();

            var json = search.GetPage().Result;
            Assert.True(json.IsApiRateLimitWarning());
        }
    }
}