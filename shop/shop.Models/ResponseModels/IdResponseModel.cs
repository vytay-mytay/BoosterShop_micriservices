using Newtonsoft.Json;

namespace shop.Models.ResponseModels
{
    public class IdResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
