using Newtonsoft.Json;

namespace Tests.Data
{
    public class RepositoryPage
    {
        public RepositoryPage()
        {
            Items = new Repository[0];
        }

        [JsonProperty(PropertyName = "items")]
        public Repository[] Items { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        public static RepositoryPage Empty(int totalResults)
        {
            return new RepositoryPage {TotalCount = totalResults};
        }
    }
}