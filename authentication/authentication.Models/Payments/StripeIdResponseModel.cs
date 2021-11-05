using Newtonsoft.Json;

namespace authentication.Models.Payments
{
    public class StripeIdResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
