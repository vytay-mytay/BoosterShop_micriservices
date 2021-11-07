using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace shop.Models.RequestModels
{
    public class StripeCreateSubscriptionRequestModel
    {
        [JsonProperty("planId")]
        [Required(ErrorMessage = "Is required")]
        public string PlanId { get; set; }
    }
}
