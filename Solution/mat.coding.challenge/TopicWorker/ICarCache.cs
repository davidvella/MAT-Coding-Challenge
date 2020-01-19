using System.Collections.Generic;

namespace mat.coding.challenge.TopicWorker
{
    public interface ICarCache
    {
        int Count { get; }
        CarInformation Read(int key);
        ICollection<CarInformation> Values();
        void Add(int key, CarInformation value);
        bool AddWithTimeout(int key, CarInformation value, int timeout);
        CarCache.AddOrUpdateStatus AddOrUpdate(int key, CarInformation value);
        void Delete(int key);
    }
}