using System;
using System.Collections.Generic;


namespace InfinityGame.ObjectPooling
{
    public class ObjectPooler<OooledType> where OooledType : IPoolable
    {
        private Dictionary<Type, Stack<OooledType>> _pool = new Dictionary<Type, Stack<OooledType>>();


        public void AddToPool(Type objectType, OooledType poolObject) 
        {
            if (!UnityEngine.Application.isPlaying)
                return;

            if (!_pool.ContainsKey(objectType))
                _pool.Add(objectType, new Stack<OooledType>());

            poolObject.PullInPreparations();
            _pool[objectType].Push(poolObject);
        }

        public bool TryGetFromPool(Type objectType , out OooledType pooledObject)
        {
            if (!_pool.TryGetValue(objectType, out Stack<OooledType> pooledObjects) || pooledObjects.Count == 0)
            {
                pooledObject = default;
                return false;
            }

            pooledObject = pooledObjects.Pop();
            pooledObject.PullOutPreparation();
            return true;
        }
    } 
}
