﻿using System.Collections.Generic;
using System.Threading;
using mat.coding.challenge.Model;

namespace mat.coding.challenge.TopicWorker
{
    /// <summary>
    /// Cache which stores the last information for the car
    /// Used implementation from https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?redirectedfrom=MSDN&amp;view=netframework-4.8
    /// </summary>
    public class CarCache
    {
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private readonly Dictionary<int, CarCoordinates> _innerCache = new Dictionary<int, CarCoordinates>();

        public int Count => _innerCache.Count;

        public CarCoordinates Read(int key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                return _innerCache[key];
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void Add(int key, CarCoordinates value)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _innerCache.Add(key, value);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public bool AddWithTimeout(int key, CarCoordinates value, int timeout)
        {
            if (_cacheLock.TryEnterWriteLock(timeout))
            {
                try
                {
                    _innerCache.Add(key, value);
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public AddOrUpdateStatus AddOrUpdate(int key, CarCoordinates value)
        {
            _cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (_innerCache.TryGetValue(key, out var result))
                {
                    if (result == value)
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        _cacheLock.EnterWriteLock();
                        try
                        {
                            _innerCache[key] = value;
                        }
                        finally
                        {
                            _cacheLock.ExitWriteLock();
                        }
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    _cacheLock.EnterWriteLock();
                    try
                    {
                        _innerCache.Add(key, value);
                    }
                    finally
                    {
                        _cacheLock.ExitWriteLock();
                    }
                    return AddOrUpdateStatus.Added;
                }
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
            }
        }

        public void Delete(int key)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _innerCache.Remove(key);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        };

        ~CarCache()
        {
            _cacheLock?.Dispose();
        }
    }
}
