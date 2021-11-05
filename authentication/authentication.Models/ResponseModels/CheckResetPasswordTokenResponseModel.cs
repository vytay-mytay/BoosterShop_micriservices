using Newtonsoft.Json;

namespace authentication.Models.ResponseModels
{
    public class CheckResetPasswordTokenResponseModel
    {
        [JsonRequired]
        public bool IsValid { get; set; }
    }
}