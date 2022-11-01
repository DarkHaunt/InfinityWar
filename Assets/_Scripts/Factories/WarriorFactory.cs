using InfinityGame.GameEntities;
using InfinityGame.ObjectPooling;
using InfinityGame.DataCaching;
using UnityEngine;


namespace InfinityGame.Factories.WarriorFactory
{
    public static class WarriorFactory
    {
        private static readonly ObjectPooler<Warrior> _warrioirPool = new ObjectPooler<Warrior>();


        public static Warrior InstantiateWarrior(Warrior prefab, Vector2 position)
        {
            if (!_warrioirPool.TryGetFromPool(prefab.PoolTag, out Warrior warrior))
            {
                warrior = MonoBehaviour.Instantiate(prefab);
                warrior.OnZeroHealth += () => _warrioirPool.AddToPool(warrior);
                warrior.OnZeroHealth += () => FractionCacher.UncacheWarrior(warrior);
            }

            warrior.transform.position = position;
            FractionCacher.CacheWarrior(warrior);

            return warrior;
        }
    }
}
