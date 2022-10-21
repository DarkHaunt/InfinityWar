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
            var literalTypeOfWarrioir = prefab.GetType();

            if (!_warrioirPool.TryGetFromPool(literalTypeOfWarrioir, out Warrior warrioirProjectile))
                warrioirProjectile = MonoBehaviour.Instantiate(prefab);

            warrioirProjectile.OnDie += () => _warrioirPool.AddToPool(literalTypeOfWarrioir, warrioirProjectile);

            return warrioirProjectile;
        }
    }
}
