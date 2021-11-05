using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace authentication.Models.RequestModels
{
    public class TokenRequestModel
    {
        [JsonProperty("token")]
        [Required(ErrorMessage = "Is required")]
        public string Token { get; set; }
    }
}
