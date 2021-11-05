using Newtonsoft.Json;

namespace authentication.Models.ResponseModels
{
    public class IdResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
