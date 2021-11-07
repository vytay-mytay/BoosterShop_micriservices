using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace shop.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserTableColumn
    {
        Id,
        FirstName,
        LastName,
        Email,
        RegisteredAt,
        IsBlocked
    }
}
