using Newtonsoft.Json;

namespace Tests.Data
{
    public class TestRepositoryPage
    {
        public TestRepositoryPage()
        {
            Items = new TestRepository[0];
        }

        [JsonProperty(PropertyName = "items")]
        public TestRepository[] Items { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        public static TestRepositoryPage Empty(int totalResults)
        {
            return new TestRepositoryPage {TotalCount = totalResults};
        }
    }
}