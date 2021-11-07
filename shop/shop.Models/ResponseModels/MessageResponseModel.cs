using Newtonsoft.Json;

namespace shop.Models.ResponseModels
{
    public class MessageResponseModel
    {
        public MessageResponseModel(string message)
        {
            Message = message;
        }

        [JsonRequired]
        public string Message { get; set; }
    }
}