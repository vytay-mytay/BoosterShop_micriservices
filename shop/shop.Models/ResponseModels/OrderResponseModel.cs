using Newtonsoft.Json;
using System.Collections.Generic;

namespace shop.Models.ResponseModels
{
    public class OrderResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("totalPrice")]
        public double TotalPrice { get; set; }

        [JsonProperty("products")]
        public List<ProductResponseModel> Products { get; set; }
    }
}
