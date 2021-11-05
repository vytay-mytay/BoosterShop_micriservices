using Newtonsoft.Json;

namespace authentication.Models.ResponseModels.Session
{
    public class SingleTokenResponseModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
