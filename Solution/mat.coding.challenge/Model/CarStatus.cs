using System;
using mat.coding.challenge.Attribute;
using Newtonsoft.Json;

namespace mat.coding.challenge.Model
{
    [Topic("carStatus")]
    public class CarStatus
    {
        /// <summary>
        /// Timestamp epoch containing milliseconds
        /// </summary>
        [JsonProperty("timestamp",Order = 1)]
        public long Timestamp { get; set; }
        /// <summary>
        /// The index of the car
        /// </summary>
        [JsonProperty("carIndex", Order = 2)]
        public int CarIndex { get; set; }
        /// <summary>
        /// SPEED|POSITION
        /// </summary>
        [JsonProperty("type", Order = 3)]
        public string Type { get; set; }
        /// <summary>
        /// The value of the message
        /// </summary>
        [JsonProperty("value", Order = 4)]
        public int Value { get; set; }

        public static CarStatus SpeedStatus(CarCoordinates topic, double speed)
        {
            return new CarStatus
            {
                CarIndex = topic.CarIndex,
                Timestamp = topic.TimeStamp,
                Type = "SPEED",
                Value = Convert.ToInt32(speed)
            };
        }

        public static CarStatus PositionStatus(CarCoordinates topic, double position)
        {
            return new CarStatus
            {
                CarIndex = topic.CarIndex,
                Timestamp = topic.TimeStamp,
                Type = "POSITION",
                Value = Convert.ToInt32(position)
            };
        }
    }


}
