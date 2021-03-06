using Newtonsoft.Json;
using shop.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace shop.Models.RequestModels
{
    public class LoginWithPhoneRequestModel : PhoneNumberRequestModel
    {
        [StringLength(50, ErrorMessage = "Password should be from 6 to 50 characters", MinimumLength = 6)]
        [CustomRegularExpression(ModelRegularExpression.REG_ONE_LATER_DIGIT, ErrorMessage = "Password should contain at least one letter and one digit")]
        [CustomRegularExpression(ModelRegularExpression.REG_NOT_CONTAIN_SPACES_ONLY, ErrorMessage = "Password can’t contain spaces only")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonProperty("accessTokenLifetime")]
        public int? AccessTokenLifetime { get; set; }
    }
}
