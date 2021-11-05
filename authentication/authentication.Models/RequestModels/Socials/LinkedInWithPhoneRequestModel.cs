using System.ComponentModel.DataAnnotations;

namespace authentication.Models.RequestModels.Socials
{
    public class LinkedInWithPhoneRequestModel : LinkedInBaseRequestModel
    {
        [RegularExpression(ModelRegularExpression.REG_PHONE, ErrorMessage = "Phone number is not in valid format")]
        [StringLength(14, ErrorMessage = "Phone number is not in valid format", MinimumLength = 13)]
        public string PhoneNumber { get; set; }
    }
}
