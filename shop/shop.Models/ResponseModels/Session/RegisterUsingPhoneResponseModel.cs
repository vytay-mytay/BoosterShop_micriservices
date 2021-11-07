using Newtonsoft.Json;

namespace shop.Models.ResponseModels.Session
{
    public class RegisterUsingPhoneResponseModel
    {
        [JsonRequired]
        public string Phone { get; set; }

        [JsonRequired]
        public bool SMSSent { get; set; }
    }
}
