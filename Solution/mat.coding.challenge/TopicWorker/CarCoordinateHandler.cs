using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mat.coding.challenge.Model;
using Microsoft.Extensions.Logging;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using GeoCoordinatePortable;
using mat.coding.challenge.Message;

namespace mat.coding.challenge.TopicWorker
{
    /// <summary>
    /// Logic for handling car coordinate topics
    /// </summary>
    public class CarCoordinateHandler : ITopicHandler<CarCoordinates>
    {
        private readonly ILogger<CarCoordinateHandler> _logger;
        private readonly CarCache _carCache;

        public CarCoordinateHandler(ILogger<CarCoordinateHandler> logger)
        {
            _logger = logger;
            _carCache = new CarCache();
        }

        /// <summary>
        /// Get the last known location and calculate speed. Then Publish back to the MQTT Broker.
        /// </summary>
        /// <param name="mqttClient">The MQTT Client</param>
        /// <param name="topic">The most recent topic</param>
        /// <returns>A Task which performs the operation</returns>
        public async Task WorkAsync(IManagedMqttClient mqttClient, CarCoordinates topic)
        {
            try
            {
                // Get lastCoordinate from key
                var lastCoordinate = _carCache.Read(topic.CarIndex);
                _carCache.AddOrUpdate(topic.CarIndex, topic);

                // Get Geo coordinate
                var lastGeoCoordinate =
                    new GeoCoordinate(lastCoordinate.Location.Latitude, lastCoordinate.Location.Longitude);
                var currentGeoCoordinate = new GeoCoordinate(topic.Location.Latitude, topic.Location.Longitude);

                // Calculate the distance and the speed
                var distance = lastGeoCoordinate.GetDistanceTo(currentGeoCoordinate);
                var speed = Speedometer.CalculateSpeedMph(distance, lastCoordinate.TimeStamp, topic.TimeStamp);

                // The response
                var speedStatus = CarStatus.SpeedStatus(topic,speed);

                var messageSpeed = MessageBuilder.CreateMessage("carStatus", JsonConvert.SerializeObject(speedStatus));
                await mqttClient.PublishAsync(messageSpeed);
                // Update the cache for next call

            }
            catch (KeyNotFoundException)
            {
                // Must be first time round, add topic to dictionary
                _carCache.AddOrUpdate(topic.CarIndex, topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
