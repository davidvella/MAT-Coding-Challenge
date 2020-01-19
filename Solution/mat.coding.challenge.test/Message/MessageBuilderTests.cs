
using mat.coding.challenge.Message;
using mat.coding.challenge.Model;
using MQTTnet;
using Xunit;

namespace mat.coding.challenge.test.Message
{
    public class MessageBuilderTests
    {
        [Fact]
        public void MessageBuilder_Returns_Message()
        {
            // Arrange
            var message = new EventMessage()
            {
                Timestamp = 1541693114862,
                Text = "Car 2 races ahead of Car 4 in a dramatic overtake."
            };

            // Act
            var sut = MessageBuilder.CreateMessage(message);

            // Assert
            var result = Assert.IsType<MqttApplicationMessage>(sut);
            Assert.Equal("events", result.Topic);
        }
    }
}
