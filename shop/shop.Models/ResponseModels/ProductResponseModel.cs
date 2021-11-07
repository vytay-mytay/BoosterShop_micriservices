using Newtonsoft.Json;

namespace shop.Models.ResponseModels
{
    public class ProductResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
