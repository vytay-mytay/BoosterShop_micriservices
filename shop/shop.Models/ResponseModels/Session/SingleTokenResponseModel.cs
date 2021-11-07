using Newtonsoft.Json;

namespace shop.Models.ResponseModels.Session
{
    public class SingleTokenResponseModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
