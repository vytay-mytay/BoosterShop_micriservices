using Newtonsoft.Json;

namespace shop.Models.ResponseModels.Session
{
    public class UserBaseResponseModel : IdResponseModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
