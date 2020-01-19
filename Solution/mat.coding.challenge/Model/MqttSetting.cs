namespace mat.coding.challenge.Model
{
    /// <summary>
    /// Settings for the MQTT Service
    /// </summary>
    public class MqttSetting
    {
        /// <summary>
        /// The address of the MQTT Broker
        /// </summary>
        public string BrokerAddress { get; set; }
        /// <summary>
        /// The port of the MQTT Broker
        /// </summary>
        public int BrokerPort { get; set; }
    }
}
