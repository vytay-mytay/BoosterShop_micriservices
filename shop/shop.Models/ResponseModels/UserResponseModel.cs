using Newtonsoft.Json;
using shop.Models.ResponseModels.Session;

namespace shop.Models.ResponseModels
{
    public class UserResponseModel : UserBaseResponseModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("isBlocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("avatar")]
        public ImageResponseModel Avatar { get; set; }
    }
}
