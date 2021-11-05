using Newtonsoft.Json;
using authentication.Models.ResponseModels.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace authentication.Models.ResponseModels
{
    public class ChatResponseModel : ChatBaseResponseModel
    {
        public ChatMessageBaseResponseModel LastItem { get; set; }
    }
}
