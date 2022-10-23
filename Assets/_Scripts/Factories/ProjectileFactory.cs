using InfinityGame.Projectiles;
using InfinityGame.ObjectPooling;
using UnityEngine;


namespace InfinityGame.Factories.ProjectileFactory
{
    public static class ProjectileFactory
    {
        private static ObjectPooler<Projectile> _projectilPool = new ObjectPooler<Projectile>();

        public static Projectile Instantiate(Projectile prefab)
        {
            if (!_projectilPool.TryGetFromPool(prefab.PoolTag, out Projectile pooledProjectile))
            {
                pooledProjectile = MonoBehaviour.Instantiate(prefab);
                pooledProjectile.OnExpluatationEnd += () => _projectilPool.AddToPool(pooledProjectile);
            }

            return pooledProjectile;
        }
    }
}