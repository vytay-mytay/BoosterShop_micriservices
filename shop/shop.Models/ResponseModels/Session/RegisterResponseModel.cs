using Newtonsoft.Json;

namespace shop.Models.ResponseModels.Session
{
    public class RegisterResponseModel
    {
        [JsonRequired]
        public string Email { get; set; }
    }
}