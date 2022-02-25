using Hephaestus.Repository.Abstraction.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Shared
{
    /// <summary>
    /// Provide object pooling for object that we want to create limited
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : IObjectPool<T>
    {
        private readonly ConcurrentBag<T> _collection;
        private readonly Func<T> _generateMethod;
        private readonly int? _maxPoolSize;
        private int _totalObject = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateMethod">The method which create a new instance of object</param>
        public ObjectPool(Func<T> generateMethod)
        {

            _collection = new ConcurrentBag<T>();
            _generateMethod = generateMethod;
            _maxPoolSize = default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateMethod">The method which create a new instance of object</param>
        /// <param name="maxPoolSize">The max count of the instance,and will throw exception when acquired instance more than the limited </param>
        /// <exception cref="ConstraintException"></exception>
        public ObjectPool(Func<T> generateMethod, int maxPoolSize)
        {

            _collection = new ConcurrentBag<T>();
            _generateMethod = generateMethod;
            _maxPoolSize = maxPoolSize;
        }

        /// <summary>
        /// Acquire an instance from pool
        /// </summary>
        /// <exception cref="ConstraintException"></exception>
        /// <returns></returns>
        public T Acquire()
        {
            if (_collection.TryTake(out T obj))
                return obj;

            //if MaxPoolSize!=null,We care limit
            if (_maxPoolSize.HasValue && _totalObject >= _maxPoolSize)
                throw new ConstraintException("Unable to acquire object from pool,All the resource in used");

            T newObject = _generateMethod();
            Interlocked.Increment(ref _totalObject);

            return newObject;
        }

        /// <summary>
        /// Release acquired instance 
        /// </summary>
        /// <param name="obj"></param>
        public void Release(T obj)
        {
            _collection.Add(obj);
        }
    }
}
