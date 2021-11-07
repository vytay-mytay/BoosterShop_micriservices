using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace shop.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ImageType
    {
        Square,
        Normal
    }
}
