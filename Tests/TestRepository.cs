using Newtonsoft.Json;

namespace Tests
{
    public class TestRepository
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}