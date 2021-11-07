using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shop.Models.RequestModels.Chat
{
    public class ReadMessagesRequestModel
    {
        [Required(ErrorMessage = "Array of Messages is required")]
        public List<int> MessageIds { get; set; }

        public int? ReadTillMessageId { get; set; }
    }
}
