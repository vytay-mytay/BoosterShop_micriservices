using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace authentication.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageType
    {
        TextMessage,
        ImageMessage
    }
}
