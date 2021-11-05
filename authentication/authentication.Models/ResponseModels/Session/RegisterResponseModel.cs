using Newtonsoft.Json;

namespace authentication.Models.ResponseModels.Session
{
    public class RegisterResponseModel
    {
        [JsonRequired]
        public string Email { get; set; }
    }
}