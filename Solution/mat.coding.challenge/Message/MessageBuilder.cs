using mat.coding.challenge.Extensions;
using MQTTnet;
using Newtonsoft.Json;

namespace mat.coding.challenge.Message
{
    public class MessageBuilder
    {
        /// <summary>
        /// Create a MqttMessage form an object.
        /// </summary>
        /// <param name="message">The object to be sent</param>
        /// <returns>A publishable message.</returns>
        public static MqttApplicationMessage CreateMessage(object message)
        {
            var topicName = message.GetType().GetTopicName();

            return new MqttApplicationMessageBuilder()
                .WithTopic(topicName)
                .WithPayload(JsonConvert.SerializeObject(message))
                .Build();
        }
    }
}
