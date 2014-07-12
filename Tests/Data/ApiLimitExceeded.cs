using Newtonsoft.Json;

namespace Tests.Data
{
    /// <summary>
    ///     Pretend to be a Github API exceeded error message.
    /// </summary>
    public class ApiLimitExceeded : TestRepository
    {
        private const string _message = "API rate limit exceeded for 1.2.3.4. (But here's the good news: Authenticated requests get a higher rate limit. Check out the documentation for more details.)";
        private const string _documentationUrl = @"https://developer.github.com/v3/#rate-limiting";
        private string _description;
        private string _name;

        [JsonProperty(PropertyName = "documentation_url")]
        public new string Description
        {
            get { return _documentationUrl; }
            set { _description = value; }
        }

        [JsonProperty(PropertyName = "message")]
        public new string Name
        {
            get { return _message; }
            set { _name = value; }
        }
    }
}