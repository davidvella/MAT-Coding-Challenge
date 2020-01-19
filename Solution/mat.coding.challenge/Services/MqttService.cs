using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mat.coding.challenge.Extensions;
using mat.coding.challenge.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;

namespace mat.coding.challenge.Services
{
    /// <summary>
    /// Service for hosting the Mqtt Client
    /// </summary>
    public class MqttService<T> : IHostedService, IDisposable
    {
        /// <summary>
        /// The injected logger 
        /// </summary>
        private readonly ILogger<MqttService<T>> _logger;
        /// <summary>
        /// The configuration options
        /// </summary>
        private readonly MqttSetting _options;
        /// <summary>
        /// The configuration options
        /// </summary>
        private readonly ITopicHandler<T> _topicHandler;
        /// <summary>
        /// The Mqtt Client
        /// </summary>
        private IManagedMqttClient _client;

        public MqttService(ILogger<MqttService<T>> logger, IOptions<MqttSetting> optionsAccessor, ITopicHandler<T> topicHandler)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
            _topicHandler = topicHandler;
        }

        /// <summary>
        /// Disposes the client.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }

        /// <summary>
        /// Start the service by building the MQTT configuration
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new MqttFactory().CreateManagedMqttClient();

            // Setup and start a managed MQTT client.
            var mqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithCommunicationTimeout(TimeSpan.FromSeconds(10))
                    .WithTcpServer(_options.BrokerAddress, _options.BrokerPort)
                    .Build())
                .Build();

            _client.UseConnectedHandler(async e =>
            {
                _logger.LogInformation("### CONNECTED WITH SERVER ###");

                await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic(typeof(T).GetTopicName()).Build());

                _logger.LogInformation("### SUBSCRIBED ###");
            });

            _client.UseApplicationMessageReceivedHandler(e =>
            {
                var obj = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                _topicHandler.Work(_client, obj);
            });

            _logger.LogInformation($"Connecting to server [{JsonConvert.SerializeObject(mqttClientOptions)}]...");
            await _client.StartAsync(mqttClientOptions);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _client.StopAsync();
        }
    }
}
