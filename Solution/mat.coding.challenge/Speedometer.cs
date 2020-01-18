
using System;
using UnitsNet;
using UnitsNet.Units;

namespace mat.coding.challenge
{
    /// <summary>
    /// Class used to calculate speed of objects.
    /// </summary>
    public class Speedometer
    {
        /// <summary>
        /// Converts Kilometers to Miles
        /// </summary>
        /// <param name="distance">The distance in kilometers</param>
        /// <returns>The distance in miles</returns>
        public static double KilometersToMiles(double distance)
        {
            if (double.IsNaN(distance))
            {
                return 0;
            }

            var kilometer = new Length(distance, LengthUnit.Kilometer);
            return kilometer.Miles;
        }

        /// <summary>
        /// Converts meters to kilometers
        /// </summary>
        /// <param name="distance">The distance in meters</param>
        /// <returns>The distance in kilometers</returns>
        public static double MetersToKiloMeters(double distance)
        {
            if (double.IsNaN(distance))
            {
                return 0;
            }

            var kilometer = new Length(distance, LengthUnit.Meter);
            return kilometer.Kilometers;
        }

        /// <summary>
        /// Calculate speed in kilometers per hour from distance in meters and two epoch times
        /// </summary>
        /// <param name="distance">The distance in meters</param>
        /// <param name="epochTime1">The First time</param>
        /// <param name="epochTime2">The Second time</param>
        /// <returns>The speed in kilometers per hour</returns>
        public static double CalculateSpeedKph(double distance, long epochTime1, long epochTime2)
        {
            // Convert the Epoch times to DateTime Offsets
            var dateTimeOffset1 = DateTimeOffset.FromUnixTimeMilliseconds(epochTime1);
            var dateTimeOffset2 = DateTimeOffset.FromUnixTimeMilliseconds(epochTime2);

            // Get the difference between two times.
            var difference = dateTimeOffset1.Subtract(dateTimeOffset2);

            // Positive time difference
            return MetersToKiloMeters(distance / Math.Abs(difference.TotalHours));
        }

        /// <summary>
        /// Calculate speed in miles per hour from distance in meters and two epoch times
        /// </summary>
        /// <param name="distance">The distance in meters</param>
        /// <param name="epochTime1">The First time</param>
        /// <param name="epochTime2">The Second time</param>
        /// <returns>The speed in miles per hour</returns>
        public static double CalculateSpeedMph(double distance, long epochTime1, long epochTime2)
        {
            return KilometersToMiles(CalculateSpeedKph(distance, epochTime1, epochTime2));
        }
    }
}
