using System.Collections;
using System.Collections.Generic;
using InfinityGame.GameEntities;
using InfinityGame.ObjectPooling;
using UnityEngine;


namespace InfinityGame.Factories.WarriorFactory
{
    public static class WarriorFactory
    {
        private static ObjectPooler<Warrior> _warrioirPool = new ObjectPooler<Warrior>();

        public static Warrior InstantiateWarrior(Warrior prefab)
        {
            if (!_warrioirPool.TryGetFromPool(prefab.PoolTag, out Warrior warrior))
            {
                warrior = MonoBehaviour.Instantiate(prefab);
                warrior.OnZeroHealth += () => _warrioirPool.AddToPool(warrior);
            }

            return warrior;
        }
    }
}
