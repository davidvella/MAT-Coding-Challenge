using Xunit;

namespace mat.coding.challenge.test
{
    public class SpeedometerTests
    {
        [Fact]
        public void CalculateSpeedKph_0_time_difference()
        {
            // Arrange
            var epochTime1 = 1541693114863;
            var epochTime2 = 1541693114863;
            var distance = 0;

            // Act
            var sut = Speedometer.CalculateSpeedKph(distance, epochTime1, epochTime2);

            // Assert
            Assert.Equal(0,sut);
        }

        [Fact]
        public void CalculateSpeedMph_0_time_difference()
        {
            // Arrange
            var epochTime1 = 1541693114863;
            var epochTime2 = 1541693114863;
            var distance = 0;

            // Act
            var sut = Speedometer.CalculateSpeedMph(distance, epochTime1, epochTime2);

            // Assert
            Assert.Equal(0, sut);
        }

        [Fact]
        public void CalculateSpeedKph_7kph_in_a_hour()
        {
            // Arrange
            var epochTime1 = 1579341600000;
            var epochTime2 = 1579338000000;
            var distance = 7000;

            // Act
            var sut = Speedometer.CalculateSpeedKph(distance, epochTime1, epochTime2);

            // Assert
            Assert.Equal(7, sut);
        }

        [Fact]
        public void CalculateSpeedMph_8mph_in_a_hour()
        {
            // Arrange
            var epochTime1 = 1579341600000;
            var epochTime2 = 1579338000000;
            var distance = 12874.8;

            // Act
            var sut = Speedometer.CalculateSpeedMph(distance, epochTime1, epochTime2);

            // Assert
            Assert.Equal(8.000049709818931, sut);
        }
    }
}
