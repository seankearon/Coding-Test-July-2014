using Newtonsoft.Json;

namespace Tests.Data
{
    public class Repository
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}