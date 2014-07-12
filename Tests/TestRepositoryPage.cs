using Newtonsoft.Json;

namespace Tests
{
    public class TestRepositoryPage
    {
        public static TestRepositoryPage Empty(int totalResults)
        {
            return new TestRepositoryPage {TotalCount = totalResults};
        }

        public TestRepositoryPage()
        {
            Items = new TestRepository[0];
        }

        [JsonProperty(PropertyName = "items")]
        public TestRepository[] Items { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }
    }
}