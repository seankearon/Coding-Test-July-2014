using System;
using GithubAPIQuery;
using Xunit;

namespace Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void RepositoryQueryReturnsValues()
        {
            var query = new RepositoryQuery("raven");
            foreach (var detail in query.GetDetails().Result)
            {
                Console.WriteLine(detail.Name + " - " + detail.Description);
            }
        }
    }
}