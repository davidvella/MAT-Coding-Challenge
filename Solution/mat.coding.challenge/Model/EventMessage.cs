using mat.coding.challenge.Attribute;
using Newtonsoft.Json;

namespace mat.coding.challenge.Model
{
    /// <summary>
    /// Message which includes event information
    /// </summary>
    [Topic("events")]
    public class EventMessage
    {
        [JsonProperty("timestamp", Order = 1)]
        public double Timestamp { get; set; }
        [JsonProperty("text", Order = 2)]
        public string Text { get; set; }
    }
}
