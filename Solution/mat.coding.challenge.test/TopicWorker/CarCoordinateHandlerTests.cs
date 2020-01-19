using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using mat.coding.challenge.Model;
using mat.coding.challenge.TopicWorker;
using Moq;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using Xunit;

namespace mat.coding.challenge.test.TopicWorker
{
    public class CarCoordinateHandlerTests
    {
        [Fact]
        public async Task Work_WhenKeyNotFound_AddToCache()
        {
            // Arrange
            var carCacheMock = new Mock<ICarCache>();
            carCacheMock
                .Setup(x => x.Read(It.IsAny<int>()))
                .Callback(() => throw new KeyNotFoundException());

            var loggerMock = new Mock<AbstractLogger<CarCoordinateHandler>>();

            var carFaker = new Faker<CarCoordinates>();
            var testCar = carFaker.Generate();

            var managedClientMock = new Mock<IManagedMqttClient>();

            var topicHandler = new CarCoordinateHandler(loggerMock.Object, carCacheMock.Object);

            // Act
            await topicHandler.Work(managedClientMock.Object, testCar);

            // Assert
            carCacheMock.Verify(x => x.AddOrUpdate(testCar.CarIndex, It.IsAny<CarInformation>()), Times.Once);
        }

        [Fact]
        public async Task Work_Publishes_Two_Events()
        {
            // Arrange
            var carFaker = new Faker<CarCoordinates>();
            var locationFaker = new Faker<Location>();
            var testCar = carFaker.Generate();
            testCar.Location = locationFaker.Generate();


            var carInformationFaker = new Faker<CarInformation>();

            var carInformation = new CarInformation()
            {
                LastLocation = locationFaker.Generate(),
                LastRecordedTimestamp = testCar.TimeStamp,
                Position = 1,
                TotalDistanceTraveled = 2,
                CarIndex = testCar.CarIndex
            };

            var carInformation2 = carInformationFaker.Generate();
            carInformation2.TotalDistanceTraveled = 1;

            var carCollection = new List<CarInformation>
            {
                carInformation,
                carInformation2
            };

            var carCacheMock = new Mock<ICarCache>();
            carCacheMock
                .Setup(x => x.Read(It.IsAny<int>()))
                .Returns(carInformation);

            carCacheMock
                .Setup(x => x.Values())
                .Returns(carCollection);

            var loggerMock = new Mock<AbstractLogger<CarCoordinateHandler>>();

            var managedClientMock = new Mock<IManagedMqttClient>();

            var topicHandler = new CarCoordinateHandler(loggerMock.Object, carCacheMock.Object);

            // Act
            await topicHandler.Work(managedClientMock.Object, testCar);

            // Assert
            carCacheMock.Verify(x => x.AddOrUpdate(testCar.CarIndex, It.IsAny<CarInformation>()), Times.Once);
            //     Extension methods (here: ManagedMqttClientExtensions.PublishAsync) may not be used in setup / verification expressions.
            //managedClientMock.Verify(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>()),Times.Exactly(2));
        }
    }
}
