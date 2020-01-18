using System.Threading.Tasks;
using MQTTnet.Extensions.ManagedClient;

namespace mat.coding.challenge.Model
{
    public interface ITopicHandler<T>
    {
        Task WorkAsync(IManagedMqttClient mqttClient, T topic);
    }
}
