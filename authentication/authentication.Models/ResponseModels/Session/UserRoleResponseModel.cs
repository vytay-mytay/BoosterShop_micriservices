using Newtonsoft.Json;

namespace authentication.Models.ResponseModels.Session
{
    public class UserRoleResponseModel : UserResponseModel
    {
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
