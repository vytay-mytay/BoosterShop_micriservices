using Newtonsoft.Json;

namespace shop.Models.ResponseModels.Session
{
    public class UserRoleResponseModel : UserResponseModel
    {
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
