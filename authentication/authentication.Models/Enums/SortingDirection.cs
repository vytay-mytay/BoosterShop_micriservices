using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace authentication.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortingDirection
    {
        Asc,
        Desc
    }
}
