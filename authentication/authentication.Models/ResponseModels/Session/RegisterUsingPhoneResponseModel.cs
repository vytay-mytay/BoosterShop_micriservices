using Newtonsoft.Json;

namespace authentication.Models.ResponseModels.Session
{
    public class RegisterUsingPhoneResponseModel
    {
        [JsonRequired]
        public string Phone { get; set; }

        [JsonRequired]
        public bool SMSSent { get; set; }
    }
}
