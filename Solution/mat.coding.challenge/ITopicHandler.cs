using System.Threading.Tasks;
using MQTTnet.Extensions.ManagedClient;

namespace mat.coding.challenge
{
    public interface ITopicHandler<T>
    {
        Task Work(IManagedMqttClient mqttClient, T topic);
    }
}
