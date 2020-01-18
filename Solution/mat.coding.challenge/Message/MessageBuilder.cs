using System.Text.Json;
using MQTTnet;

namespace mat.coding.challenge.Message
{
    public class MessageBuilder
    {
        public static MqttApplicationMessage CreateMessage(string topic, string payload)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(payload);

            return new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(bytes)
                .Build();
        }
    }
}
