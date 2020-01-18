using System;
using Newtonsoft.Json;

namespace mat.coding.challenge.Model
{
    public class CarStatus
    {
        /// <summary>
        /// Timestamp epoch containing milliseconds
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        /// <summary>
        /// The index of the car
        /// </summary>
        [JsonProperty("carIndex")]
        public int CarIndex { get; set; }
        /// <summary>
        /// SPEED|POSITION
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// The value of the message
        /// </summary>
        [JsonProperty("value")]
        public long Value { get; set; }

        public static CarStatus SpeedStatus(CarCoordinates topic, double speed)
        {
            return new CarStatus
            {
                CarIndex = topic.CarIndex,
                Timestamp = topic.TimeStamp,
                Type = "SPEED",
                Value = Convert.ToInt64(speed)
            };
        }
    }


}
