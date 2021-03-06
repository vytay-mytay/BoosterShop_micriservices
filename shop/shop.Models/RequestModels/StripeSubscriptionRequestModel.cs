using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace shop.Models.RequestModels
{
    public class StripeSubscriptionRequestModel
    {
        [JsonProperty("subscriptionId")]
        [Required(ErrorMessage = "Is required")]
        public string SubscriptionId { get; set; }
    }
}
