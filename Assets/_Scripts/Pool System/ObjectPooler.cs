using System;
using UnityEngine;
using System.Collections.Generic;


namespace InfinityGame.ObjectPooling
{
    public class ObjectPooler<PooledType> where PooledType : IPoolable
    {
        private Dictionary<string, Stack<PooledType>> _pool = new Dictionary<string, Stack<PooledType>>();


        public void AddToPool(PooledType poolObject)
        {
            if (!Application.isPlaying)
                return;

            if (!_pool.ContainsKey(poolObject.PoolTag))
                _pool.Add(poolObject.PoolTag, new Stack<PooledType>());

            poolObject.PullInPreparations();
            _pool[poolObject.PoolTag].Push(poolObject);
        }

        public bool TryGetFromPool(string poolTag, out PooledType poolObject)
        {
            if (_pool.TryGetValue(poolTag, out Stack<PooledType> pooledObjects) && pooledObjects.Count != 0)
            {
                poolObject = pooledObjects.Pop();
                poolObject.PullOutPreparation();
                return true;
            }

            poolObject = default;
            return false;
        }
    }
}
