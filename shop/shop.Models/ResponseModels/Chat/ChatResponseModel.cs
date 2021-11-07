using Newtonsoft.Json;
using shop.Models.ResponseModels.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace shop.Models.ResponseModels
{
    public class ChatResponseModel : ChatBaseResponseModel
    {
        public ChatMessageBaseResponseModel LastItem { get; set; }
    }
}
