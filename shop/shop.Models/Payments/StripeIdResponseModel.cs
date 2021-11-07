using Newtonsoft.Json;

namespace shop.Models.Payments
{
    public class StripeIdResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
