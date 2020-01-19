using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mat.coding.challenge.Model;
using Microsoft.Extensions.Logging;
using MQTTnet.Extensions.ManagedClient;
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
        // Create a new concurrent dictionary.
        private readonly ICarCache _carCache;

        public CarCoordinateHandler(ILogger<CarCoordinateHandler> logger)
        {
            _logger = logger;
            _carCache = new CarCache();
        }

        public CarCoordinateHandler(ILogger<CarCoordinateHandler> logger, ICarCache carCache)
        {
            _logger = logger;
            _carCache = carCache;
        }

        /// <summary>
        /// Get the last known location and calculate speed. Then Publish back to the MQTT Broker.
        /// </summary>
        /// <param name="mqttClient">The MQTT Client</param>
        /// <param name="topic">The most recent topic</param>
        /// <returns>A Task which performs the operation</returns>
        public async Task Work(IManagedMqttClient mqttClient, CarCoordinates topic)
        {
            try
            {
                // Get lastCoordinate from key
                var carInformation = _carCache.Read(topic.CarIndex);

                // Get Geo coordinate
                var lastGeoCoordinate = new GeoCoordinate(carInformation.LastLocation.Latitude, carInformation.LastLocation.Longitude);
                var currentGeoCoordinate = new GeoCoordinate(topic.Location.Latitude, topic.Location.Longitude);

                // Calculate the distance and the speed
                var distance = lastGeoCoordinate.GetDistanceTo(currentGeoCoordinate);
                var speed = Speedometer.CalculateSpeedMph(distance, carInformation.LastRecordedTimestamp,
                    topic.TimeStamp);

                // using the total distance driven
                var carValues = _carCache.Values().ToList();
                var position = carValues.OrderByDescending(x => x.TotalDistanceTraveled)
                                   .Select(x => x.CarIndex).ToList().IndexOf(topic.CarIndex) + 1;

                //  has the car position changed
                if (position != carInformation.Position)
                {
                    //  who is now in that position?
                    var carInPreviousPos = carValues.FirstOrDefault(x => x.Position == position);

                    if (carInPreviousPos != null)
                    {
                        var eventStatus = new EventMessage()
                        {
                            Timestamp = topic.TimeStamp,
                            Text =
                                $"Car {carInPreviousPos.CarIndex} races ahead of Car {carInformation.CarIndex} in a dramatic overtake."
                        };
                        var messageEvent = MessageBuilder.CreateMessage(eventStatus);

                        //  send event for the overtake
                        await mqttClient.PublishAsync(messageEvent);
                    }
                }

                // Set information car information
                carInformation.TotalDistanceTraveled += distance;
                carInformation.LastRecordedTimestamp = topic.TimeStamp;
                carInformation.LastLocation = topic.Location;
                carInformation.Position = position;
                _carCache.AddOrUpdate(topic.CarIndex, carInformation);

                // The response
                var messageSpeed = MessageBuilder.CreateMessage(CarStatus.SpeedStatus(topic, speed));
                var messagePosition = MessageBuilder.CreateMessage(CarStatus.PositionStatus(topic, position));

                await mqttClient.PublishAsync(messageSpeed);
                await mqttClient.PublishAsync(messagePosition);

            }
            catch (KeyNotFoundException)
            {
                // Must be first time round, add topic to dictionary
                var carInformation = new CarInformation()
                {
                    LastLocation = topic.Location,
                    LastRecordedTimestamp = topic.TimeStamp,
                    Position = 0,
                    TotalDistanceTraveled = 0,
                    CarIndex = topic.CarIndex
                };

                _carCache.AddOrUpdate(topic.CarIndex, carInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
