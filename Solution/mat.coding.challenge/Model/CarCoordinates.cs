
using mat.coding.challenge.Attribute;
using Newtonsoft.Json;

namespace mat.coding.challenge.Model
{
    /// <summary>
    /// MQTT Topic information
    /// </summary>
    [Topic("carCoordinates")]
    public class CarCoordinates
    {
        /// <summary>
        /// Timestamp epoch containing milliseconds
        /// </summary>
        [JsonProperty("timestamp")] public long TimeStamp { get; set; }
        /// <summary>
        /// The index of the car
        /// </summary>
        [JsonProperty("carIndex")] public int CarIndex { get; set; }
        /// <summary>
        /// The current index of the car
        /// </summary>
        [JsonProperty("location")] public Location Location { get; set; }
    }

    /// <summary>
    /// Json object to hold the latitude and longitude
    /// </summary>
    public class Location
    {
        [JsonProperty("lat")] public double Latitude { get; set; }
        [JsonProperty("long")] public double Longitude { get; set; }
    }

}
