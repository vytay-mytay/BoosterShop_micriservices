using System;
using System.ComponentModel.DataAnnotations;

namespace shop.Models.RequestModels.Chat
{
    public class ChatMessagesListRequestModel : PaginationBaseRequestModel
    {
        public DateTime? StartDate { get; set; }
    }
}
