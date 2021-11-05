using Newtonsoft.Json;

namespace authentication.Models.ResponseModels.Session
{
    public class LoginResponseModel
    {
        [JsonProperty("user")]
        public UserRoleResponseModel User { get; set; }

        [JsonRequired]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TokenResponseModel Token { get; set; }
    }
}