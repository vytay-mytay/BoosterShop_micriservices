using Newtonsoft.Json;
using System.Collections.Generic;

namespace authentication.Models.ResponseModels.Payments
{
    public class PaymentMethodsResponseModel
    {
        [JsonProperty("paymentMethods")]
        public List<string> PaymentMethods { get; set; }
    }
}
