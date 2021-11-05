using System;
using System.ComponentModel.DataAnnotations;

namespace authentication.Models.RequestModels.Chat
{
    public class ChatMessagesListRequestModel : PaginationBaseRequestModel
    {
        public DateTime? StartDate { get; set; }
    }
}
