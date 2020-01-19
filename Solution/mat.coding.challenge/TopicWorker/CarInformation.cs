using mat.coding.challenge.Model;

namespace mat.coding.challenge.TopicWorker
{
    public class CarInformation
    {
        /// <summary>
        /// Timestamp epoch containing milliseconds
        /// </summary>
        public long LastRecordedTimestamp { get; set; }
        /// <summary>
        /// The total distance traveled by the car
        /// </summary>
        public double TotalDistanceTraveled { get; set; }
        /// <summary>
        /// The last location of the car
        /// </summary>
        public Location LastLocation { get; set; }
        /// <summary>
        /// The total distance traveled by the car
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// The index of the car
        /// </summary>
        public int CarIndex { get; set; }
    }
}
