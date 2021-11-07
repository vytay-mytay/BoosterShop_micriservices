using Newtonsoft.Json;

namespace shop.Models.ResponseModels
{
    public class CheckResetPasswordTokenResponseModel
    {
        [JsonRequired]
        public bool IsValid { get; set; }
    }
}