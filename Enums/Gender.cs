using System.Text.Json.Serialization;

namespace Arowolo_Delivery_Project.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male,
        Female
    }
}
