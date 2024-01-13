using System.Text.Json.Serialization;

namespace startup_trial.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        Wok,
        Pizza,
        Soup,
        Dessert,
        Drink
    }
}
